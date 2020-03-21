using GameServices;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (Input.GetKeyDown(KeyCode.A))
            {
                //GameServicesLocator.Instance.SceneServiceProvider.LoadSceneAsync("ComplexScene", () =>
                //{
                //    Debug.Log("WTF");
                //});

                //GameServicesLocator.Instance.SceneServiceProvider.LoadScene("ComplexScene", () =>
                //{
                //    Debug.Log("WTF");
                //});
                SceneManager.LoadScene("ComplexScene");
            }
        }
    }
}