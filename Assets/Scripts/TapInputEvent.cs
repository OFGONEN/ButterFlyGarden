using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TapInputEvent", menuName = "FF/Event/TapInputEvent")]
public class TapInputEvent : GameEvent
{
    [HideInInspector]
    public int tapCount;

}
