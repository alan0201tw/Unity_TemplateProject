using FatshihLib.Event;
using GameServices.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameServices.Interface
{
    public class EndedTouchEventArgs : EventArgs
    {
        public bool isTouchMoved;
        public Vector2 position;

        public EndedTouchEventArgs(bool isTouchMoved, Vector2 position)
        {
            this.isTouchMoved = isTouchMoved;
            this.position = position;
        }
    }

    public class MotionEventArgs : EventArgs
    {
        public Vector2 motion;

        public MotionEventArgs(Vector2 motion)
        {
            this.motion = motion;
        }
    }

    public interface IMobileInputServiceProvider
    {
        /// <summary>
        /// This event is triggered whenever a Touch is in Ended or Canceled phase.
        /// The first bool is whether the ended touch moved or not. If you want a touch "click" event,
        /// do an if statement on that.
        /// The second Vector2 is the position of the ended touch.
        /// </summary>
        event UnityEventHandler<EndedTouchEventArgs> OnTouchEnded;

        /// <summary>
        /// This event is triggered whenever a Touch is moving.
        /// Please notice that touches that starts by touching a UI element is excluded, otherwise you
        /// might receive this event when player is tuning volume.
        /// The vector2 is the motion
        /// </summary>
        event UnityEventHandler<MotionEventArgs> OnTouchMoving;

        event UnityEventHandler<MotionEventArgs> OnJoystickMoving;
    }
}

namespace GameServices.MobileInputService
{
    public class MobileInputServiceProvider : MonoBehaviour, IMobileInputServiceProvider
    {
        [SerializeField]
        private JoyStick joystick;

        public event UnityEventHandler<EndedTouchEventArgs> OnTouchEnded;
        public event UnityEventHandler<MotionEventArgs> OnTouchMoving;

        public event UnityEventHandler<MotionEventArgs> OnJoystickMoving;
        public event Action OnJoystickIdle;

        // use this to determine if the touch start on an UI element
        private Dictionary<int, TouchState> touchStateDictionary = new Dictionary<int, TouchState>();

        private void Awake()
        {
            ProvideService();
        }

        private void ProvideService()
        {
            GameServicesLocator.Instance.MobileInputServiceProvider = this;
        }

        private void Update()
        {
            // if player is using joystick
            if (joystick.IsDraggingJoystick)
            {
                Vector2 motion = joystick.Motion;

                if (OnJoystickMoving != null)
                {
                    MotionEventArgs eventArgs = new MotionEventArgs(motion);
                    OnJoystickMoving.Invoke(this, eventArgs);
                }
            }
            else
            {
                // if player is not using joystick
                if (OnJoystickIdle != null)
                    OnJoystickIdle.Invoke();
            }

            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (joystick.IsDraggingJoystick && touch.fingerId == joystick.DraggingTouchID)
                        continue;

                    if (touch.phase == TouchPhase.Began)
                    {
                        // IsPointerOverGameObject will only work when the touch is in began state and
                        // the default function with id = -1 won't work, so we need to assign fingerId

                        // invalid touch : over UI
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            if (!touchStateDictionary.ContainsKey(touch.fingerId))
                                touchStateDictionary.Add(touch.fingerId, TouchState.Invalid);
                            else
                                touchStateDictionary[touch.fingerId] = TouchState.Invalid;
                        }
                        else // valid touch
                        {
                            if (!touchStateDictionary.ContainsKey(touch.fingerId))
                                touchStateDictionary.Add(touch.fingerId, TouchState.Valid);
                            else
                                touchStateDictionary[touch.fingerId] = TouchState.Valid;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (OnTouchEnded != null)
                        {
                            bool isInvalid = IsTouchInDictionaryInvalid(touch.fingerId);
                            if (isInvalid)
                                return;

                            bool isTouchMoved = (touchStateDictionary[touch.fingerId] == TouchState.Moved);

                            if (OnTouchEnded != null)
                            {
                                EndedTouchEventArgs eventArgs = new EndedTouchEventArgs(isTouchMoved, touch.position);

                                OnTouchEnded.Invoke(this, eventArgs);
                            }
                        }
                        // this touch leaves screen, so set it to invalid
                        touchStateDictionary[touch.fingerId] = TouchState.Invalid;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        // if this touch is invalid, don't reset it to moved
                        bool isInvalid = IsTouchInDictionaryInvalid(touch.fingerId);
                        if (isInvalid)
                            return;

                        touchStateDictionary[touch.fingerId] = TouchState.Moved;

                        if (OnTouchMoving != null)
                        {
                            MotionEventArgs eventArgs = new MotionEventArgs(touch.deltaPosition);
                            OnTouchMoving.Invoke(this, eventArgs);
                        }
                    }
                }
            }
        }

        private bool IsTouchInDictionaryInvalid(int fingerID)
        {
            // if the touch didn't exist in the dictionary
            if (!touchStateDictionary.ContainsKey(fingerID)) return true;
            // or if the touch exist, but is invalid
            if (touchStateDictionary[fingerID] == TouchState.Invalid) return true;

            // if none above, it's valid or moved
            return false;
        }

    }

    public enum TouchState : byte
    {
        Valid, Moved, Invalid
    }
}