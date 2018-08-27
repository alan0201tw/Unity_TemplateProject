using UnityEngine;
using GameServices;

public class Tester : MonoBehaviour
{
    [SerializeField]
    private float volume;
    [SerializeField]
    private LanguageType languageType;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameServicesLocator.Instance.AudioServiceProvider.SetVolume(volume);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameServicesLocator.Instance.SceneServiceProvider.LoadSceneAsync(1, () =>
            {
                Debug.Log("Load Scene Complete.");
            });
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameServicesLocator.Instance.LanguageServiceProvider.SetLanguageType(languageType);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType());
            Debug.Log(GameServicesLocator.Instance.AudioServiceProvider.GetCurrentVolume());
        }
    }
}