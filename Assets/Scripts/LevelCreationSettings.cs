using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCreationSettings", menuName = "FF/Data/LevelSettings")]
public class LevelCreationSettings : ScriptableObject
{
    public int maxLevelCount;
    public float horizontalDistance = 1.5f;
    public float verticalDistance = 1.5f;

    public float butterFlyDistanceToLily = 0.2f;
    public float frogFlyDistanceToLily = 0.1f;

    public float butterFlyFlyDuration = 0.5f;

    public float butterFlyIdleAnimRepeatMin = 6f;
    public float butterFlyIdleAnimRepeatMax = 10f;

}

