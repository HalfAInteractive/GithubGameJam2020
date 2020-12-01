using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    InputControls input = null;
    bool isPaused = false;

    public PlayerController PlayerController { get; private set; } = null;

    LevelManager levelManager = null;

    Level currentLevel;
    public event Action<Level> OnWarpSceneBegin = delegate { };
    public event Action<Level> OnWarpSceneFinished = delegate { };

    private void Awake()
    {
        input = new InputControls();
        
    }

    private void Start()
    {
        PlayerController = GetComponentInChildren<PlayerController>();
        levelManager = GetComponent<LevelManager>();
        levelManager.OnLevelLoadFinished += WarpSceneFinished;

        // this is when you are testing scenes and you already dragged in two scenes.
        // i am sure this will cause problems
        if (SceneManager.sceneCount > 1) return;

        InitiateLevelChange(Level.Tutorial);
        
    }

    private void OnDestroy()
    {
        levelManager.OnLevelLoadFinished -= WarpSceneFinished;
    }

    private void OnEnable()
    {
        input.InGame.Pause.performed += ctx => DeterminePauseState();
        input.Enable();
    }

    private void OnDisable()
    {
        input.InGame.Pause.performed -= ctx => DeterminePauseState();
        input.Disable();
    }

    void DeterminePauseState()
    {
        if (!isPaused)
        {
            Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        PlayerController.DisableControls();
        levelManager.ShowPause();
    }

    public void Unpause()
    {
        isPaused = false;
        PlayerController.EnableControls();
        levelManager.ExitPause();

    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(ReturnToMainMenuCoroutine());
    }

    IEnumerator ReturnToMainMenuCoroutine()
    {
        GlobalVolumeController.PlayVisualFX.FadeOut();
        yield return new WaitForSeconds(1.25f);
        levelManager.GoToMainMenu();
    }

    public void InitiateLevelChange(Level level)
    {
        currentLevel = level;
        OnWarpSceneBegin(level);

        if (level != Level.Tutorial)
        {
            TimerManager.Empty();
            GlobalVolumeController.PlayVisualFX.WarpSpeedEffects(intensity: -1f, duration: 1f, inTransitionSpeed: 10f, outTransitionSpeed: 1f);
        }

        levelManager.GoToLevel(level);
        PlayerController.transform.position = Vector3.zero;

        // if you teleport the player with the code above, you have to call OnTargetObjectWarped to disable the smoothing at that instance.
        // i could probably make this look cleaner or something, but this works for now.
        PlayerController.PlayerCamera.GetComponent<CinemachineBrain>().ActiveVirtualCamera.OnTargetObjectWarped(PlayerController.transform, Vector3.zero);
    }

    public void WarpSceneFinished()
    {
        OnWarpSceneFinished(currentLevel);
    }
}
