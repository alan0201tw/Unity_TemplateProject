using System.Collections;
using UnityEngine;

namespace FatshihDebug
{
    public class FPSVisualizer : MonoBehaviour
    {
        private float frameRate;
        private float averageFPS;

        private int passedFrameCount;

        [SerializeField]
        private float FPSUpdateRate = 0.1f;

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = 12
            };
            GUI.Label(new Rect(10, 10, 200, 20), "FPS : " + frameRate, style);
            GUI.Label(new Rect(10, 30, 200, 20), "averageFPS : " + averageFPS, style);
        }

        private void Start()
        {
            StartCoroutine(UpdateFPS());

            passedFrameCount = 0;
        }

        private void Update()
        {
            passedFrameCount++;

            averageFPS = passedFrameCount / Time.time;
        }

        private IEnumerator UpdateFPS()
        {
            while (true)
            {
                frameRate = 1 / Time.deltaTime;

                yield return new WaitForSeconds(FPSUpdateRate);
            }
        }
    }
}