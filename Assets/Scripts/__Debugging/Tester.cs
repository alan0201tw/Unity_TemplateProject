using UnityEngine;
using GameServices;

namespace FatshihDebug
{
    public class Tester : MonoBehaviour
    {
        private void Start()
        {
            GameServicesLocator.Instance.MobileInputServiceProvider.OnTouchEnded += (boo, p) =>
            {
                Debug.Log("OnTouchEnded");
            };

            GameServicesLocator.Instance.MobileInputServiceProvider.OnTouchMoving += (boo, p) =>
            {
                Debug.Log("OnTouchMoving");
            };

            GameServicesLocator.Instance.MobileInputServiceProvider.OnJoystickMoving += (b, arg) =>
            {
                Debug.Log("OnJoystickMoving : Motion = " + arg.motion);
            };
        }
    }
}