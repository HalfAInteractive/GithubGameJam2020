using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject basePauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject confirmExitMenu;

    RectTransform basePauseMenuRect;

    float bottomHideScreen = -600f, middleScreen = 0f;
    float moveDuration = .25f;

    GameObject currentMenu = null;

    GameManager gameManager = null;

    InputControls inputControls;

    private void Awake()
    {
        basePauseMenuRect = basePauseMenu.GetComponent<RectTransform>();
        basePauseMenuRect.localPosition = new Vector3(0f, -600f, 0f);

        inputControls = new InputControls();
    }

    private void Start()
    {
        gameManager = HelperFunctions.FindGameManagerInBaseScene();
    }


    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        currentMenu = basePauseMenu;

        inputControls.UI.Back.performed += ctx => Back();
        inputControls.Enable();

        basePauseMenuRect
            .DOLocalMoveY(middleScreen, moveDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputControls.UI.Back.performed -= ctx => Back();
        inputControls.Disable();
    }

    public void ReturnToGame()
    {
#if UNITY_EDITOR
        if(gameManager == null)
        {
            Debug.LogWarning("WRN: GM is not found. You must be testing the scene. Otherwise, your GM is null!");
        }
#endif

        Sequence seq = DOTween.Sequence();
        seq.Append(basePauseMenuRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => basePauseMenu.SetActive(false));
        seq.AppendCallback(() => gameManager?.Unpause());
    }

    public void ShowOptionsMenu()
    {
        currentMenu = optionsMenu;
        RectTransform currentRect = currentMenu.GetComponent<RectTransform>();
        currentRect.localPosition = new Vector3(0f, bottomHideScreen, 0f);

        Sequence seq = DOTween.Sequence();
        seq.Append(basePauseMenuRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => basePauseMenu.SetActive(false));
        seq.AppendCallback(() => currentMenu.SetActive(true));
        seq.Append(currentRect.DOLocalMoveY(middleScreen, moveDuration));
    }

    public void ShowPauseMenu()
    {
        basePauseMenuRect.localPosition = new Vector3(0f, bottomHideScreen, 0f);

        RectTransform currentRect = currentMenu.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(currentRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => currentMenu.SetActive(false));
        seq.AppendCallback(() => basePauseMenu.SetActive(true));
        seq.AppendCallback(() => currentMenu = basePauseMenu);
        seq.Append(basePauseMenuRect.DOLocalMoveY(middleScreen, moveDuration));
    }

    public void ShowConfirmExitMenu()
    {
        currentMenu = confirmExitMenu;
        RectTransform currentRect = currentMenu.GetComponent<RectTransform>();
        currentRect.localPosition = new Vector3(0f, bottomHideScreen, 0f);

        Sequence seq = DOTween.Sequence();
        seq.Append(basePauseMenuRect.DOLocalMoveY(bottomHideScreen, moveDuration));
        seq.AppendCallback(() => basePauseMenu.SetActive(false));
        seq.AppendCallback(() => currentMenu.SetActive(true));
        seq.Append(currentRect.DOLocalMoveY(middleScreen, moveDuration));
    }

    // this works out because we don't have a layer of menus.
    // if so, then we would have ot restructure the menu to a stack or something.
    public void Back()
    {
        if (currentMenu == basePauseMenu) return;

        ShowPauseMenu();
    }

    public void ExitGame()
    {
        gameManager.ReturnToMainMenu();
        gameObject.SetActive(false); // make menu disappear
    }
}
