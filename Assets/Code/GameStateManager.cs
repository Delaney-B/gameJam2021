using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
    private static GameStateManager _instance;

    public static GameStateManager GetInstance() {
        if (!_instance) {
            _instance = FindObjectOfType<GameStateManager>();
            if (!_instance) {
                throw new Exception(
                    "Unable to find GameStateManager instance! Add the globals scene to the hierarchy access it."
                );
            }
        }

        return _instance;
    }

    private void Awake() {
        if (SceneManager.sceneCount == 1) {
            LoadMenuScene();
        }
    }

    public void LoadMenuScene() {
        StartCoroutine(LoadScene_CR("MenuScene"));
    }

    public void LoadGameScene() {
        StartCoroutine(LoadScene_CR("GameScene"));
    }

    private IEnumerator LoadScene_CR(string sceneName) {
        // TODO animate loading indicator?

        for (var i = 0; i < SceneManager.sceneCount; i++) {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.name.Equals("GlobalsScene")) {
                Debug.Log("Unloading " + scene.name);
                var unloadSceneAsync = SceneManager.UnloadSceneAsync(scene);
                yield return unloadSceneAsync;
            }
        }

        var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadSceneAsync.allowSceneActivation = true;
        yield return loadSceneAsync;

        // TODO stop animating loading indicator?
    }
}
