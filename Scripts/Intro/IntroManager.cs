using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] GameObject studioNameWords = null;
    [SerializeField] GameObject faceWord = null;
    [SerializeField] GameObject warningWords = null;
    [SerializeField] AudioFade sfx = null;

    private void Awake()
    {
        Application.targetFrameRate = -1;
        faceWord.SetActive(false);
        warningWords.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        StartCoroutine(IntroCoroutine());
    }

    IEnumerator IntroCoroutine()
    {
        GlobalVolumeController.PlayVisualFX.FadeIn();

        yield return new WaitForSeconds(4.4f);
        warningWords.SetActive(true);

        yield return new WaitForSeconds(5.5f);
        warningWords.SetActive(false);
        studioNameWords.SetActive(false);

        yield return new WaitForSeconds(2f);
        faceWord.SetActive(true);

        yield return new WaitForSeconds(2f);
        sfx.FadeOut();
        GlobalVolumeController.PlayVisualFX.FadeOut();

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(GameConstants.MAIN_MENU_SCENE);
    }
}
