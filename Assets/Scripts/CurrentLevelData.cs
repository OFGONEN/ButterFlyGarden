using System.Collections;
using System.Collections.Generic;
using FFStudio;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentLevelData", menuName = "FF/Data/CurrentLevelData")]
public class CurrentLevelData : ScriptableObject
{
    public int currentLevel;
    public LevelData levelData;
    public GameEvent levelLoadedEvent;

    public void LoadCurrentLevelData()
    {
        levelData = Resources.Load<LevelData>("LevelData_" + currentLevel);
    }
    public bool FindTarGetButterFly(Color colorOne, Color colorTwo, out int dataIndex)
    {
        var _targetButterFlies = levelData.targetButterFlyDatas;

        for (int i = 0; i < _targetButterFlies.Count; i++)
        {
            var _find = true;

            var _targetButterFlyData = _targetButterFlies[i];

            _find &= _targetButterFlyData.butterFlyColors.FindSameColor(colorOne);
            _find &= _targetButterFlyData.butterFlyColors.FindSameColor(colorTwo);

            if (_find)
            {
                dataIndex = i;
                return true;
            }
        }

        dataIndex = 0;
        return false;
    }
}
