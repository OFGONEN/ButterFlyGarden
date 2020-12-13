using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentLevelData", menuName = "FF/Data/CurrentLevelData")]
public class CurrentLevelData : ScriptableObject
{
    public int currentLevel;
    public LevelData levelData;

    public void LoadCurrentLevelData()
    {
        levelData = Resources.Load<LevelData>("LevelData_" + currentLevel);
    }
}
