using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level002Completion : MonoBehaviour
{
    [SerializeField] GameObject warpPortal;
    [SerializeField] AmmoTargetSpawner ammoTargetSpawner;
    [SerializeField] int goalOfTargetsToHit = 10;
    [SerializeField] Enemy enemy;

    TMP_Text instructions;
    GameManager gameManager = null;
    WaitForSeconds waitBetweenWords = new WaitForSeconds(6f);

    IEnumerator Start()
    {
        instructions = GetComponentInChildren<TMP_Text>();
        instructions.gameObject.SetActive(false);
        gameManager = HelperFunctions.FindGameManagerInBaseScene();

        yield return null;

        List<Enemy> badPracticesEnemies = new List<Enemy>();
        for (int i = 0; i < 15; i++)
        {
            var temp = Instantiate(enemy, transform.position, Quaternion.identity);
            temp.gameObject.SetActive(false);
            badPracticesEnemies.Add(temp);
            yield return null;
        }

        instructions.text = "Episode 2 : Pew Pew Pew!";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        var waitCheck = new WaitForSeconds(1f);
        while(ammoTargetSpawner.NumOfConfirmedHit < goalOfTargetsToHit)
        {
            yield return waitCheck;
        }

        instructions.text = "Hmmm...let's try something else.";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        var playerCamTransform = gameManager.PlayerController.PlayerCamera.transform;

        var tempEnemy = Instantiate(enemy, playerCamTransform.position + Random.onUnitSphere * 200f, Quaternion.identity);
        //enemy.transform.SetPositionAndRotation(playerCamTransform.position + Random.onUnitSphere * 200f, Quaternion.identity);
        tempEnemy.gameObject.SetActive(true);
        while(tempEnemy.IsAlive)
        {
            yield return waitCheck;
        }

        instructions.text = "All right, not bad.";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        instructions.text = "Wait, what's that over there!?!";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        foreach(var temp in badPracticesEnemies)
        {
            temp.transform.SetPositionAndRotation(playerCamTransform.position + Random.onUnitSphere * 250f, Quaternion.identity);
            temp.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }

        //int numOfDead = 0, numOfEnemies = badPracticesEnemies.Count;
        //while(numOfDead != numOfEnemies)
        //{
        //    int counter = 0;
        //    foreach(var badPracticeEnemy in badPracticesEnemies)
        //    {
        //        if (!badPracticeEnemy.IsAlive) counter++; // i know there's a linq thing that does this but forgot
        //    }

        //    print($"num of alive {counter}");
        //    numOfDead = counter;
        //    yield return waitCheck;
        //}


        instructions.text = "Run while you still can! You can't win!!!";
        instructions.gameObject.SetActive(true);
        yield return waitBetweenWords;
        instructions.gameObject.SetActive(false);

        var spawnPosition = playerCamTransform.position + playerCamTransform.forward * 250f;
        var spawnRotation = playerCamTransform.rotation;
        warpPortal.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        warpPortal.SetActive(true);
    }
}
