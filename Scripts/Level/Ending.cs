using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    TMP_Text endingWords = null;
    GameManager gameManager = null;
    WaitForSeconds waitBetweenWords = new WaitForSeconds(6f);

    IEnumerator Start()
    {
        gameManager = HelperFunctions.FindGameManagerInBaseScene();
        endingWords = GetComponentInChildren<TMP_Text>();
        endingWords.gameObject.SetActive(false);

        var waitBetweenInstructions = new WaitForSeconds(3f);
        yield return new WaitForSeconds(2f);

        endingWords.text = "Ending : Half A'd it";
        endingWords.gameObject.SetActive(true);
        yield return waitBetweenWords;
        endingWords.gameObject.SetActive(false);

        endingWords.text = "While the game didn't go as planned,";
        endingWords.gameObject.SetActive(true);
        yield return waitBetweenWords;
        endingWords.gameObject.SetActive(false);

        endingWords.text = "We hope you had fun.";
        endingWords.gameObject.SetActive(true);
        yield return waitBetweenWords;
        endingWords.gameObject.SetActive(false);

        endingWords.text = "Thanks for taking the playing our game!";
        endingWords.gameObject.SetActive(true);
        yield return waitBetweenWords;
        endingWords.gameObject.SetActive(false);


        endingWords.text = "Returning back to main menu.";
        endingWords.gameObject.SetActive(true);
        yield return waitBetweenWords;
        endingWords.gameObject.SetActive(false);

        yield return new WaitForSeconds(4f);

        gameManager.ReturnToMainMenu();
    }
}