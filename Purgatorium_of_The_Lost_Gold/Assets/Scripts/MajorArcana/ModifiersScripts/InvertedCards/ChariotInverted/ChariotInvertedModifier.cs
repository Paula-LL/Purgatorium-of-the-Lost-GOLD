using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChariotInvertedModifier : ScriptableObject
{
    public abstract void ApplyChariotInvertedCardModifier(PlayerStats.Movement movement);

}
