using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Inventory : MonoBehaviour
{
    [SerializeField]
    public TMP_Text pentaclesQuantityText;  
    
    private void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI() { 
        pentaclesQuantityText.text = InventoryController.Instance.inventory.coinsTotal.ToString();
    } 

}
