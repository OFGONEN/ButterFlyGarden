﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterFly : OccupyingEntity
{
    public Color color;

    private static int ColorShaderID = Shader.PropertyToID("_Color");

    public OccupyingEntitySet occupyingEntitySet;
    public ButterFlySet butterFlySet;
    public LevelCreationSettings creationSettings;
    public GameObject mergedButterFly;

    private void OnEnable()
    {
        if (!hasData) return;


        occupyingEntitySet.itemDictionary.Add(mapCord, this);
        occupyingEntitySet.itemList.Add(this);

        butterFlySet.itemDictionary.Add(mapCord, this);
        butterFlySet.itemList.Add(this);
    }
    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetColor(ColorShaderID, color);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
    }
    private void OnDisable()
    {
        occupyingEntitySet.itemList.Remove(this);
        occupyingEntitySet.itemDictionary.Remove(mapCord);

        butterFlySet.itemList.Remove(this);
        butterFlySet.itemDictionary.Remove(mapCord);
    }

    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }
    public bool MoveToPlatform(Vector2 additiveCord)
    {
        var _newCord = mapCord + additiveCord;

        PlatformEntity _platform = platformEntity.GetNeighborPlatform(additiveCord);

        if (_platform == null || _platform.occupingEntity is Frog) return false;

        var _targetPosition = _platform.transform.position;
        _targetPosition.y = creationSettings.butterFlyDistanceToLily;
        transform.position = _targetPosition;


        OccupyPlatform(_newCord, _platform);

        return true;
    }
    public void OccupyPlatform(Vector2 newMapCord, PlatformEntity platform)
    {
        platformEntity.occupingEntity = null;

        mapCord = newMapCord;
        platform.inComingEntity = this;
        platformEntity = platform;
    }
    public virtual void Encounter()
    {
        if (platformEntity.occupingEntity == null)
        {
            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;
        }
        else if (platformEntity.occupingEntity is MergedButterFly)
        {
            platformEntity.inComingEntity = null;
            var _mergedButterFly = platformEntity.occupingEntity as MergedButterFly;

            _mergedButterFly.inputButterFlies.Add(this);
            _mergedButterFly.Merge();

            gameObject.SetActive(false);
        }
        else if (platformEntity.occupingEntity is ButterFly)
        {
            platformEntity.inComingEntity = null;
            var _occupyingButterFly = platformEntity.occupingEntity as ButterFly;

            //create merged butterfly
            var _mergedButterFly = GameObject.Instantiate(mergedButterFly, transform.position, transform.rotation, transform.parent);
            var _mergedButterFlyComponent = _mergedButterFly.GetComponent<MergedButterFly>();

            _mergedButterFlyComponent.mapCord = mapCord;
            _mergedButterFlyComponent.platformEntity = platformEntity;
            _mergedButterFlyComponent.SetData();

            _mergedButterFlyComponent.inputButterFlies.Add(_occupyingButterFly);
            _mergedButterFlyComponent.inputButterFlies.Add(this);
            _mergedButterFlyComponent.Merge();

            platformEntity.occupingEntity = _mergedButterFlyComponent;

            _occupyingButterFly.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    public Color GetColor()
    {
        return color;
    }
}
