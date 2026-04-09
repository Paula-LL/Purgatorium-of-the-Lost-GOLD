using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LootPool : MonoBehaviour
{
    public GameObject spawnedCardPrefab;
    public List<CardsLoot> lootCards = new List<CardsLoot>();

    public CardsLoot GetRandomCard()
    {
        float randomNum = Random.Range(0f, 100f);
        float counter = 0f;

        foreach (var card in lootCards)
        {
            if (randomNum < counter + card.dropChance)
                return card;

            counter += card.dropChance;
        }

        return lootCards[lootCards.Count - 1];
    }

    public void InstantiateCardLoot(Transform spawnPoint)
    {
        InstantiateCardLoot(spawnPoint.position);
    }

    public void InstantiateCardLoot(Vector3 spawnPosition)
    {
        CardsLoot droppedCard = GetRandomCard();

        if (spawnedCardPrefab == null)
        {
            Debug.LogError("No prefab assigned");
            return;
        }

        Instantiate(spawnedCardPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Spawned card: " + droppedCard.name);
    }

#if UNITY_EDITOR
    [ContextMenu("InstantiateDebug")]
#endif
    public void InstantiateDebug()
    {
        for (int i = 0; i < 1000; i++)
        {
            Debug.Log(GetRandomCard().name);
        }
    }

    //call to car spawning on another script with: if public LootPool lootPool
    //lootPool.InstantiateCardLoot(spawnPointTransform);
}


//public CardsLoot dropChanceLoot; 

/*List<CardsLoot> GetDroppedCard() {
    float randomNumber = Random.Range(1f, 100.01f);
    List<CardsLoot> possibleCards = new List<CardsLoot>();

    foreach (CardsLoot cards in lootCards) {
        if (randomNumber <= cards.dropChance) { 
            possibleCards.Add(cards);
            return possibleCards; 
        }
    }

    if (possibleCards.Count > 0) { 
        CardsLoot droppedCard = possibleCards[Random.Range(0,possibleCards.Count)];
    }

   // Mathf.Max(dropChanceLoot.dropChance);

    return null;//placeholder, return card with highest drop rate.  
}*/