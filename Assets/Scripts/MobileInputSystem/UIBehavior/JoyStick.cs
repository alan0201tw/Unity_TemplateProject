using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameServices.MobileInputService
{
    // use this to strict gameObject to UI elements
    [RequireComponent(typeof(Graphic))]
    public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField]
        private int movementRadius = 50;

        public event Action OnStartDragging;
        public event Action OnStopDragging;

        public Vector2 Motion { get; private set; }
        public bool IsDraggingJoystick { get; private set; }
        public int DraggingTouchID { get; private set; }

        // for Editor
        private Vector3 m_initialPosition;

        public int MovementRadius { get { return movementRadius; } }
        public Vector3 InitialPosition { get { return m_initialPosition; } }

        private bool m_isUsingWorldPosition;
        private Camera m_canvasCamera;

        private void Start()
        {
            m_initialPosition = transform.position;

            var renderMode = GetComponentInParent<Canvas>().renderMode;
            if (renderMode == RenderMode.ScreenSpaceOverlay)
            {
                m_isUsingWorldPosition = false;
            }
            else
            {
                m_isUsingWorldPosition = true;
                m_canvasCamera = GetComponentInParent<Canvas>().worldCamera;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 touchPosition;
            if (!m_isUsingWorldPosition)
            {
                touchPosition = eventData.position;
            }
            else
            {
                touchPosition = m_canvasCamera.ScreenToWorldPoint(eventData.position);
            }

            Vector2 delta = new Vector2(touchPosition.x - InitialPosition.x, touchPosition.y - InitialPosition.y);

            if (delta.magnitude > movementRadius)
            {
                delta = delta.normalized * movementRadius;
            }

            transform.position = new Vector3(InitialPosition.x + delta.x, InitialPosition.y + delta.y, InitialPosition.z);

            // assign motion and state variables for external usage
            // normalizing motion will be necessary
            Motion = delta / movementRadius;
            IsDraggingJoystick = true;
            DraggingTouchID = eventData.pointerId;

            if (OnStartDragging != null)
            {
                OnStartDragging.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.position = InitialPosition;

            StartCoroutine(CancelDragging());
        }

        private IEnumerator CancelDragging()
        {
            yield return new WaitForEndOfFrame();
            // reset variables to idle state
            IsDraggingJoystick = false;
            Motion = Vector2.zero;

            if (OnStopDragging != null)
            {
                OnStopDragging.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Do nothing, but without this OnPointerUp won't work!!!
        }
    }
}