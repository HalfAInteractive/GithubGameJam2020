using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject optionsMenu = null;
    [SerializeField] GameObject creditsMenu = null;

    [SerializeField] AudioFade music = null;
    [SerializeField] AudioFade sfx = null;

    RectTransform mainMenuRect = null;

    float bottomHideScreen = -600f, middleScreen = 0f;
    float moveDuration = .25f;

    GameObject currentMenu = null, prevMenu = null;

    InputControls inputControls = null;

    private void Awake()
    {
        mainMenuRect = mainMenu.GetComponent<RectTransform>();
        mainMenuRect.localPosition = new Vector3(0f, -600f, 0f);

        inputControls = new InputControls();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        GlobalVolumeController.PlayVisualFX.FadeIn();

        currentMenu = mainMenu;
        inputControls.UI.Back.performed += ctx => Back();
        inputControls.Enable();

        mainMenuRect
            .DOLocalMoveY(middleScreen, moveDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    private void OnDisable()
    {
        inputControls.UI.Back.performed -= ctx => Back();
        inputControls.Disable();
    }

    public void ShowOptionsMenu()
    {
        currentMenu = optionsMenu;
        RectTransform currentRect = currentMenu.GetComponent<RectTransform>();
        currentRect.localPosition = new Vector3(0f, bottomHideScreen, 0f);

        Sequence seq = DOTween.Sequence();
        seq.Append(mainMenuRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => mainMenu.SetActive(false));
        seq.AppendCallback(() => currentMenu.SetActive(true));
        seq.Append(currentRect.DOLocalMoveY(middleScreen, moveDuration));
    }

    public void ShowMainMenu()
    {
        mainMenuRect.localPosition = new Vector3(0f, bottomHideScreen, 0f);

        RectTransform currentRect = currentMenu.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(currentRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => currentMenu.SetActive(false));
        seq.AppendCallback(() => mainMenu.SetActive(true));
        seq.AppendCallback(() => currentMenu = mainMenu);
        seq.Append(mainMenuRect.DOLocalMoveY(middleScreen, moveDuration));
    }

    public void ShowCreditsMenu()
    {
        currentMenu = creditsMenu;
        RectTransform creditsRect = currentMenu.GetComponent<RectTransform>();
        creditsRect.localPosition = new Vector3(0f, bottomHideScreen, 0f);

        Sequence seq = DOTween.Sequence();
        seq.Append(mainMenuRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => mainMenu.SetActive(false));
        seq.AppendCallback(() => currentMenu.SetActive(true));
        seq.Append(creditsRect.DOLocalMoveY(middleScreen, moveDuration));

    }

    // this works out because we don't have a layer of menus.
    // if so, then we would have ot restructure the menu to a stack or something.
    public void Back()
    {
        if (currentMenu == mainMenu) return;

        ShowMainMenu();
    }

    public void PlayGame()
    {
        GlobalVolumeController.PlayVisualFX.FadeOut();
        inputControls.Disable();

        GetComponent<Canvas>().enabled = false;
        music.FadeOut();
        sfx.FadeOut();

        StartCoroutine(PlayGameCoroutine());
    }

    IEnumerator PlayGameCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(GameConstants.BASE_SCENE);
    }

    public void ExitGame()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
