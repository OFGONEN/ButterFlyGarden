using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCreationSettings", menuName = "FF/Data/LevelSettings")]
public class LevelCreationSettings : ScriptableObject
{
    public float horizontalDistance = 1.5f;
    public float verticalDistance = 1.5f;

    public float butterFlyDistanceToLily = 0.2f;
    public float frogFlyDistanceToLily = 0.1f;

}

