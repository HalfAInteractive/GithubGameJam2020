using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level001Completion : MonoBehaviour
{
    [SerializeField] GameObject warpPortal;

    TMP_Text instructions;
    GameManager gameManager = null;
    WaitForSeconds waitBetweenWords = new WaitForSeconds(6f);

    void Awake()
    {
        warpPortal.SetActive(false);
        
    }

    IEnumerator Start()
    {
        yield return null;

        instructions = GetComponentInChildren<TMP_Text>();
        instructions.gameObject.SetActive(false);
        gameManager = HelperFunctions.FindGameManagerInBaseScene();

        yield return new WaitForSeconds(1f);

        instructions.text = "Episode 1 : The Ring";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        WaitForSeconds waitCheck = new WaitForSeconds(1f);
        var playerStarCollector = gameManager.PlayerController.GetComponent<StarCollector>();
        while(!playerStarCollector.HasRing)
        {
            yield return waitCheck;
        }

        instructions.text = "If you like it, collect enough star dust to put a ring on it.";
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
