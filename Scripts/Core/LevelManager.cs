using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public event Action OnLevelLoadBegin = delegate { };
    public event Action OnLevelLoadFinished = delegate { };

    IEnumerator Start()
    {
        yield return null;
        //yield return ShowTestSceneCoroutine();
    }

    IEnumerator ShowTestSceneCoroutine()
    {
        if (!SceneManager.GetSceneByName(GameConstants.TEST_SCENE).isLoaded)
        {
            var newScene = SceneManager.LoadSceneAsync(GameConstants.TEST_SCENE, LoadSceneMode.Additive);
            while (!newScene.isDone)
            {
                yield return null;
            }

            var currentLevelScene = SceneManager.GetSceneByName(GameConstants.TEST_SCENE);
            if (currentLevelScene.isLoaded)
            {
                SceneManager.SetActiveScene(currentLevelScene);
            }
        }
    }


    public void ShowPause()
    {
        if (!(SceneManager.GetSceneByName(GameConstants.PAUSE_SCENE).isLoaded))
        {
            Scene pause = SceneManager.GetSceneByName(GameConstants.PAUSE_SCENE);
            SceneManager.LoadSceneAsync(GameConstants.PAUSE_SCENE, LoadSceneMode.Additive);
        }
    }

    public void ExitPause()
    {
        if (SceneManager.GetSceneByName(GameConstants.PAUSE_SCENE).isLoaded)
        {
            SceneManager.UnloadSceneAsync(GameConstants.PAUSE_SCENE);
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(GameConstants.MAIN_MENU_SCENE);
    }

    public void GoToLevel(Level level)
    {
        if (level == Level.Tutorial)
        {
            StartCoroutine(GoToTutorialCoroutine());
        }
        else
        {
            StartCoroutine(GoToLevelCoroutine(level));
        }
    }

    IEnumerator GoToTutorialCoroutine()
    {
        OnLevelLoadBegin();
        string sceneNameToLoad = HelperFunctions.GetLevelNameByEnum(Level.Tutorial);
        var newScene = SceneManager.LoadSceneAsync(sceneNameToLoad, LoadSceneMode.Additive);

        float startTime = Time.realtimeSinceStartup;
        while(!newScene.isDone)
        {
            yield return null;
        }

        var currentLoadedLevelScene = SceneManager.GetSceneByName(sceneNameToLoad);
        if (currentLoadedLevelScene.isLoaded)
        {
            SceneManager.SetActiveScene(currentLoadedLevelScene);
        }

#if UNITY_EDITOR
        print($"Load time for tutorial: {Time.realtimeSinceStartup - startTime}");
#endif

        GC.Collect();
        OnLevelLoadFinished();
    }

    IEnumerator GoToLevelCoroutine(Level level)
    {
        OnLevelLoadBegin();

        yield return new WaitForSeconds(1f); // pause a bit to make the particles play for a bit

        float startTime = Time.realtimeSinceStartup;

        var currScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currScene);

        string sceneNameToLoad = HelperFunctions.GetLevelNameByEnum(level);
        var newScene =  SceneManager.LoadSceneAsync(sceneNameToLoad, LoadSceneMode.Additive);
        while(!newScene.isDone)
        {
            //print("loading new scnene");
            yield return null;
        }

        var currentLoadedLevelScene = SceneManager.GetSceneByName(sceneNameToLoad);
        if(currentLoadedLevelScene.isLoaded)
        {
            SceneManager.SetActiveScene(currentLoadedLevelScene);
        }

#if UNITY_EDITOR
        print($"Load time for {level}: {Time.realtimeSinceStartup - startTime}");
#endif

        GC.Collect();
        yield return new WaitForSeconds(2f);

        OnLevelLoadFinished();
    }
}
