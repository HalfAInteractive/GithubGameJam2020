using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Item Drop")]
public class ItemDrop : ScriptableObject
{
    [Header("Random chances for droppings to get chosen")]

    [Tooltip("Common will trigger 3rd to uncommon then Rare if hits.")]
    [SerializeField] [Range(0, 100)] int chanceOfCommon = 60;

    [Tooltip("Uncommon will trigger second to Rare if hits.")]
    [SerializeField] [Range(0, 100)] int chanceOfUncommon = 40;

    [Tooltip("Rare will trigger first if hits.")]
    [SerializeField] [Range(0, 100)] int chanceOfRare = 10;

    [Header("The Items")]
    [SerializeField] List<GameObject> commonDrops = null;
    [SerializeField] List<GameObject> uncommonDrops = null;
    [SerializeField] List<GameObject> rareDrops = null;


    // rare has highest priority if chosen, then uncommon and lastly common.
    public GameObject GetRandomDrop()
    {
        GameObject item = null;
        int randVal = Random.Range(0, 100);

        if (HelperFunctions.IsWithinInclusive(randVal, 0, chanceOfRare))
        {
            item = GetRandomRareDrop();
        }
        else if(HelperFunctions.IsWithinInclusive(randVal, 0, chanceOfUncommon))
        {
            item = GetRandomUncommonDrop();
        }
        else if (HelperFunctions.IsWithinInclusive(randVal, 0, chanceOfCommon))
        {
            item = GetRandomCommonDrop();
        }

        return item;
    }


    public GameObject GetRandomCommonDrop()
    {
        int idx = Random.Range(0, commonDrops.Count);
        return commonDrops[idx];
    }

    public GameObject GetRandomUncommonDrop()
    {
        int idx = Random.Range(0, uncommonDrops.Count);
        return uncommonDrops[idx];
    }

    public GameObject GetRandomRareDrop()
    {
        int idx = Random.Range(0, rareDrops.Count);
        return rareDrops[idx];
    }
}