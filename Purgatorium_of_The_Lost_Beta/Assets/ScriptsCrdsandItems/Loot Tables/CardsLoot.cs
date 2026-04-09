using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardsLoot : ScriptableObject
{
    public GameObject cardPrefabs;
    public float dropChance;

    public CardsLoot(GameObject cardPrefabs, int dropChance) { 
        this.cardPrefabs = cardPrefabs;
        this.dropChance = dropChance;
    }
}
