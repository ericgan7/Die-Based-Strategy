using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager> {

    private float loadProgress;
    public float LoadingPorgress
    {
        get { return loadProgress; }
    }

    public void loadScene(string sceneName)
    {
        if (!CheckSceneIsLoaded(sceneName))
        {
            StartCoroutine(LoadScenesInOrder(sceneName));
        }
        else
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }

    private IEnumerator LoadScenesInOrder(string sceneName)
    {
        //could yield a loading screen
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        while (!scene.isDone)
        {
            loadProgress = Mathf.Clamp01(scene.progress / 0.9f) * 100;
            if (scene.progress >= 0.9f)
            {
                scene.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    private bool CheckSceneIsLoaded(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).IsValid()){
            return true;
        }
        else
        {
            return false;
        }
    }
}
