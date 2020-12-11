﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "FF/Data/LevelData")]
public class LevelData : ScriptableObject
{
    public Vector2 leftDown;
    public Vector2 size;
    public List<LilyData> lilyDatas = new List<LilyData>();
    public List<ButterFlyData> butterFlyDatas = new List<ButterFlyData>();
}

[System.Serializable]
public struct LilyData
{
    public GameObject levelObject;
    public Vector2 position;
    public float direction;
}
[System.Serializable]
public struct ButterFlyData
{
    public GameObject levelObject;
    public Vector2 position;
    public float direction;
    public Color butterFlyColor;

}