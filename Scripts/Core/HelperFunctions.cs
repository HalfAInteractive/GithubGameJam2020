using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelperFunctions
{
    /// <summary>
    /// Be sure if you're calling this that you are doing so not in Awake and if you're doing this at Start,
    /// you will have to yield a frame before referencing it in things like Update that would happen the following frame.
    /// </summary>
    /// <returns></returns>
    public static GameManager FindGameManagerInBaseScene()
    {
        Scene baseScene = SceneManager.GetSceneByName(GameConstants.BASE_SCENE);
        if (baseScene.isLoaded) // not sure if we really need to check it bceause it has to be loaded in general to play the game.
        {
            var baseGameObjects = baseScene.GetRootGameObjects();
            foreach (var go in baseGameObjects)
            {
                if (go.TryGetComponent(out GameManager gm))
                {
                    return gm;
                }
            }
        }

        return null;
    }

    public static string GetLevelNameByEnum(Level level)
    {
        switch (level)
        {
            case Level.Level001: return GameConstants.LEVEL_001;
            case Level.Level002: return GameConstants.LEVEL_002;
            case Level.Level003: return GameConstants.LEVEL_003;
            case Level.Level004: return GameConstants.LEVEL_004;
            case Level.Ending: return GameConstants.ENDING_SCENE;
            case Level.Tutorial: return GameConstants.TUTORIAL_SCENE;
            default: return GameConstants.BASE_SCENE;
        }
    }

    public static bool IsWithinInclusive(int val, int low, int high)
    {
        if (low > high)
        {
            int temp = low;
            low = high;
            high = temp;
        }

        return val >= low && val <= high;
    }

    public static bool IsWithinInclusive(float val, float low, float high)
    {
        if (low > high)
        {
            float temp = low;
            low = high;
            high = temp;
        }

        return val >= low && val <= high;
    }
}
