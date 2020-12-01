using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMessage : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject textMessage;

    private void Start()
    {
        gameManager.PlayerController.OnPlayerKilled += ShowDeathMessage;
    }

    private void OnDisable()
    {
        gameManager.PlayerController.OnPlayerKilled -= ShowDeathMessage;
    }

    void ShowDeathMessage()
    {
        
        StartCoroutine(DelayToMainMenuCoroutine());
    }

    IEnumerator DelayToMainMenuCoroutine()
    {
        yield return new WaitForSeconds(4f);
        textMessage.SetActive(true);

        yield return new WaitForSeconds(10f);
        gameManager.ReturnToMainMenu();
    }
}
