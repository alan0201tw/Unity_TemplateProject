using GameServices;
using UnityEngine;

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
                Debug.Log("OnTouchMoving, motion = " + p.motion);
            };
        }

        private void Update()
        {

        }
    }
}