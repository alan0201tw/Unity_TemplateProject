using GameServices.Interface;
using UnityEngine;

namespace GameServices
{
    public class GameServicesLocator
    {
        private static GameServicesLocator s_instance = new GameServicesLocator();
        public static GameServicesLocator Instance
        {
            get { return s_instance; }
        }
        // set constructor to private to avoid external instancing
        private GameServicesLocator()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("GameServicesLocator : Initialize");
        }

        public IAudioServiceProvider audioServiceProvider = new AudioService.AudioServiceProvider();
        public ISceneServiceProvider sceneServiceProvider = new SceneService.SceneServiceProvider();
        public ILanguageServiceProvider languageServiceProvider = new LanguageService.LanguageServiceProvider();
    }
}