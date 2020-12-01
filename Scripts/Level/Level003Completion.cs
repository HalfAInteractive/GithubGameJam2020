using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level003Completion : MonoBehaviour
{
    [SerializeField] GameObject warpPortal;

    TMP_Text instructions;
    GameManager gameManager = null;
    WaitForSeconds waitBetweenWords = new WaitForSeconds(5f);

    IEnumerator Start()
    {
        instructions = GetComponentInChildren<TMP_Text>();
        instructions.gameObject.SetActive(false);
        gameManager = HelperFunctions.FindGameManagerInBaseScene();

        yield return new WaitForSeconds(2f);

        instructions.text = "Episode 3 : Questionable";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        instructions.text = "Well, that was something.";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        instructions.text = "I am not going to lie. We ran out of time to finishing the game.";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        instructions.text = "But there's a lot of neat star dust to collect!";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        instructions.text = "Here's the warp if you want to move on.";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        var playerCamTransform = gameManager.PlayerController.PlayerCamera.transform;
        var spawnPosition = playerCamTransform.position + playerCamTransform.forward * 250f;
        var spawnRotation = playerCamTransform.rotation;
        warpPortal.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        warpPortal.SetActive(true);
    }
}
