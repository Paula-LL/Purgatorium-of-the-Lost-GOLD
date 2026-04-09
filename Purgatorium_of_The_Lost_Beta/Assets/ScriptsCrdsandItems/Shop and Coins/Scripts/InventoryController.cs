using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class InventoryController : MonoBehaviour
{

    //public TMP_Text coinsTotalText; 
    public PlayerInventory inventory;

    public Inventory inv; 

    public static InventoryController Instance;

    public List<PentacleModifier> pentaclesCoinsList = new List<PentacleModifier>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new PlayerInventory();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //PickUpCurrency(amount); 
            Debug.Log("Picked up Minor Arcana: Pentacle");
        }
    }

    /*public void PickUpCurrency(int amount) {
        inventory.coinsTotal += amount;
        coinsTotalText.text = inventory.coinsTotal.ToString();
        inv.UpdateInventoryUI();
    }*/

    internal void AddModifier(PentacleModifier pentaclesCoins, bool updateUI = true)
    {
        pentaclesCoinsList.Add(pentaclesCoins);
        ApplyPentacleModifiers(inventory);
    }

    private void ApplyPentacleModifiers(PlayerInventory inventoryPentacles, bool updateUI = true)
    {
        foreach (PentacleModifier modifier in pentaclesCoinsList)
        {
            modifier.ApplyPentaclesCardModifier(inventoryPentacles);
        }
        if (updateUI)
           inv.UpdateInventoryUI(); 
    }

    internal void SetCurrentPentacleCoins(int value)
    {
        inventory.coinsTotal = value;
        inv.UpdateInventoryUI(); 
    }
}

[System.Serializable]
public class PlayerInventory {
    public int coinsTotal;

    public PlayerInventory() { }

    public PlayerInventory(int coinsTotal)
    {
        this.coinsTotal = coinsTotal;
    }
}