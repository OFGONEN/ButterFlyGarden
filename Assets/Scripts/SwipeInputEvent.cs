using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwipeInputEvent", menuName = "FF/Event/SwipeInputEvent")]
public class SwipeInputEvent : GameEvent
{
    [HideInInspector]
    public Vector2 swipeDirection = Vector2.zero;
}
