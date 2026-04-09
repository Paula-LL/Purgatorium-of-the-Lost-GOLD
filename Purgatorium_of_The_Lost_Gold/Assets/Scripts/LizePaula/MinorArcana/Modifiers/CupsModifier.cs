using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CupsModifier : ScriptableObject
{
    public abstract void ApplyCupsCardModifier(Player_controller playerController);
}
