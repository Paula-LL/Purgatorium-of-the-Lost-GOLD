using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChariotNormalModifier : ScriptableObject
{
    public abstract void ApplyChariotNormalCardModifier(PlayerStats.Movement movement);
}
