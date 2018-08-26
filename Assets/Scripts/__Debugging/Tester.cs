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
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameServicesLocator.Instance.audioServiceProvider.SetVolume(volume);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            GameServicesLocator.Instance.sceneServiceProvider.LoadSceneAsync(1, () =>
            {
                Debug.Log("Load Scene Complete.");
            });
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            GameServicesLocator.Instance.languageServiceProvider.SetLanguageType(languageType);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(GameServicesLocator.Instance.languageServiceProvider.GetCurrentLanguageType());
            Debug.Log(GameServicesLocator.Instance.audioServiceProvider.GetCurrentVolume());
        }
    }
}