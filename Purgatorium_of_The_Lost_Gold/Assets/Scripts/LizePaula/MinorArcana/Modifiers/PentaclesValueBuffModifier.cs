using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minor Arcana/Pentacles")]
public class PentaclesValueBuffModifier : PentacleModifier
{
    public int pentaclesCoinsAmount;
    public override void ApplyPentaclesCardModifier(PlayerInventory pentacles)
    {
        pentacles.coinsTotal += pentaclesCoinsAmount;
    }
}
