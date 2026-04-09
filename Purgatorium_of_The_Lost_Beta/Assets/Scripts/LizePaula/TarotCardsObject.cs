using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TarotCardsObject : ScriptableObject
{
    public abstract void Apply(GameObject target);//target refering to player
}
