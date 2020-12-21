using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntegerEvent", menuName = "FF/Event/IntGameEvent")]
public class IntGameEvent : GameEvent
{
    [HideInInspector]
    public int intValue;
}
