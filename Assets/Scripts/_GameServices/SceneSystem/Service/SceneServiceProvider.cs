using GameServices.Interface;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.Interface
{
    public interface ISceneServiceProvider
    {
        void LoadScene(int sceneIndex, Action onLoadComplete = null);
        void LoadScene(string sceneName, Action onLoadComplete = null);
        void LoadSceneAsync(int sceneIndex, Action onLoadComplete = null);
        void LoadSceneAsync(string sceneName, Action onLoadComplete = null);
    }
}

namespace GameServices.SceneService
{
    public class SceneServiceProvider : ISceneServiceProvider
    {
        private GameObject m_dummyGameObject;
        public GameObject DummyGameObject
        {
            get
            {
                if (m_dummyGameObject == null)
                    m_dummyGameObject = new GameObject("DummyGameObject for SceneServiceProvider");
                return m_dummyGameObject;
            }
        }

        private bool m_isLoadingScene;

        public int CurrentSceneIndex
        {
            get { return SceneManager.GetActiveScene().buildIndex; }
        }

        public string CurrentSceneName
        {
            get { return SceneManager.GetActiveScene().name; }
        }

        public void LoadScene(int sceneIndex, Action onLoadComplete = null)
        {
            // if there is a coroutine running, ignore this request
            if (m_isLoadingScene)
                return;

            // since this is blocking, no need to set up the loading flag
            SceneManager.LoadScene(sceneIndex);
            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }

        public void LoadScene(string sceneName, Action onLoadComplete = null)
        {
            if (m_isLoadingScene)
                return;

            // since this is blocking, no need to set up the loading flag
            SceneManager.LoadScene(sceneName);
            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }

        public void LoadSceneAsync(int sceneIndex, Action onLoadComplete = null)
        {
            // if currently is loading scene, ignore request
            if (m_isLoadingScene == true)
                return;

            CoroutineRunner coroutineRunner = DummyGameObject.AddComponent<CoroutineRunner>();
            m_isLoadingScene = true;
            coroutineRunner.StartCoroutine(LoadSceneCoroutine(sceneIndex, onLoadComplete));
        }

        public void LoadSceneAsync(string sceneName, Action onLoadComplete = null)
        {
            // if currently is loading scene, ignore request
            if (m_isLoadingScene == true)
                return;

            CoroutineRunner coroutineRunner = DummyGameObject.AddComponent<CoroutineRunner>();
            m_isLoadingScene = true;
            coroutineRunner.StartCoroutine(LoadSceneCoroutine(sceneName, onLoadComplete));
        }

        private IEnumerator LoadSceneCoroutine(int sceneIndex, Action onLoadComplete)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            // when the scene completes loading itself to memory
            // it still needs to call Awake and Start on all objects in that scene
            // this process cannot be cut and must be done in one cycle
            // so LoadAsync still yields a stall, but shorter.
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            m_isLoadingScene = false;

            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, Action onLoadComplete)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            m_isLoadingScene = false;

            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }
    }
}