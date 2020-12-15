﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class LevelManager : MonoBehaviour
{
    public Camera mainCamera;
    public EventListenerDelegateResponse restartLevelEventListener;
    public LevelCreationSettings creationSettings;
    public CurrentLevelData currentLevelData;
    public PlatformEntitySet platformEntitySet;

    public OccupyingEntitySet occupyingEntitySet;
    public ButterFlySet butterFlySet;
    public FrogSet frogSet;

    public Transform parentTransform;

    private void OnEnable()
    {
        restartLevelEventListener.OnEnable();
    }
    private void Start()
    {
        restartLevelEventListener.response = RestartLevel;

        CreateLevel();
        SetUpLevel();
    }
    private void OnDisable()
    {

        restartLevelEventListener.OnDisable();
    }

    public void CreateLevel()
    {
        var _levelData = currentLevelData.levelData;

        for (int i = 0; i < _levelData.lilyDatas.Count; i++)
        {
            var _lilyData = _levelData.lilyDatas[i];

            var _lily = GameObject.Instantiate(_lilyData.levelObject);
            _lily.transform.SetParent(parentTransform);

            _lily.transform.position = new Vector3(_lilyData.position.x * creationSettings.horizontalDistance,
             0,
             _lilyData.position.y * creationSettings.verticalDistance);

            _lily.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            var _lilyComponent = _lily.GetComponent<Lily>();
            _lilyComponent.mapCord = _lilyData.mapCord;
            _lilyComponent.direction = _lilyData.direction.ReturnV2FromUnSignedAngle();
            _lilyComponent.SetData();

        }

        for (int i = 0; i < _levelData.butterFlyDatas.Count; i++)
        {
            var _butterFlyData = _levelData.butterFlyDatas[i];

            var _butterFly = GameObject.Instantiate(_butterFlyData.levelObject);
            _butterFly.transform.SetParent(parentTransform);

            _butterFly.transform.position = new Vector3(_butterFlyData.position.x * creationSettings.horizontalDistance,
             creationSettings.butterFlyDistanceToLily, // Stands on top of lily
             _butterFlyData.position.y * creationSettings.verticalDistance);

            _butterFly.transform.rotation = Quaternion.Euler(0, _butterFlyData.direction, 0);

            var _butterflyComponent = _butterFly.GetComponent<ButterFly>();
            _butterflyComponent.butterFlyData = _butterFlyData;
            _butterflyComponent.color = _butterFlyData.butterFlyColor;
            _butterflyComponent.mapCord = _butterFlyData.mapCord;
            _butterflyComponent.direction = _butterFlyData.direction.ReturnV2FromUnSignedAngle();
            _butterflyComponent.SetData();
            // _butterflyComponent.
        }

        for (int i = 0; i < _levelData.frogDatas.Count; i++)
        {
            var _frogData = _levelData.frogDatas[i];

            var _frog = GameObject.Instantiate(_frogData.levelObject);
            _frog.transform.SetParent(parentTransform);

            _frog.transform.position = new Vector3(_frogData.position.x * creationSettings.horizontalDistance,
             creationSettings.frogFlyDistanceToLily, // Stands on top of lily
             _frogData.position.y * creationSettings.verticalDistance);

            _frog.transform.rotation = Quaternion.Euler(0, _frogData.direction, 0);

            var _frogComponent = _frog.GetComponent<Frog>();
            _frogComponent.frogData = _frogData;
            _frogComponent.color = _frogData.frogColor;
            _frogComponent.mapCord = _frogData.mapCord;
            _frogComponent.direction = _frogData.direction.ReturnV2FromUnSignedAngle();
            _frogComponent.SetData();
        }

        parentTransform.position = new Vector3(-creationSettings.horizontalDistance * (_levelData.size.x - 1) / 2,
        0,
        -creationSettings.verticalDistance * (_levelData.size.y - 1) / 2);

        var _cameraPos = mainCamera.transform.position;
        _cameraPos.y = _levelData.size.x * 3;
        mainCamera.transform.position = _cameraPos;
    }
    public void SetUpLevel()
    {
        for (int i = 0; i < platformEntitySet.itemList.Count; i++)
        {
            platformEntitySet.itemList[i].FindNeighbors();
            platformEntitySet.itemList[i].FindOccupyingEntity();
        }
    }
    public void RestartLevel()
    {
        var _childCount = parentTransform.childCount;

        for (int i = 0; i < _childCount; i++)
        {
            parentTransform.GetChild(i).gameObject.SetActive(true);
        }

        List<ButterFly> _butterFlies = new List<ButterFly>(butterFlySet.itemList.Count);

        for (int i = 0; i < butterFlySet.itemList.Count; i++)
        {
            var _butterFly = butterFlySet.itemList[i];
            var _butterFlyData = _butterFly.butterFlyData;

            _butterFlies.Add(_butterFly);

            _butterFly.transform.localPosition = new Vector3(_butterFlyData.position.x * creationSettings.horizontalDistance,
             creationSettings.butterFlyDistanceToLily, // Stands on top of lily
             _butterFlyData.position.y * creationSettings.verticalDistance);

            _butterFly.transform.rotation = Quaternion.Euler(0, _butterFlyData.direction, 0);
            _butterFly.transform.localScale = Vector3.one;

            _butterFly.color = _butterFlyData.butterFlyColor;
            _butterFly.mapCord = _butterFlyData.mapCord;
            _butterFly.direction = _butterFlyData.direction.ReturnV2FromUnSignedAngle();
        }



        List<Frog> _frogs = new List<Frog>(frogSet.itemList.Count);
        for (int i = 0; i < frogSet.itemList.Count; i++)
        {
            var _frog = frogSet.itemList[i];
            var _frogData = _frog.frogData;

            _frogs.Add(_frog);

            _frog.transform.localPosition = new Vector3(_frogData.position.x * creationSettings.horizontalDistance,
             creationSettings.frogFlyDistanceToLily, // Stands on top of lily
             _frogData.position.y * creationSettings.verticalDistance);

            _frog.transform.rotation = Quaternion.Euler(0, _frogData.direction, 0);

            _frog.color = _frogData.frogColor;
            _frog.mapCord = _frogData.mapCord;
            _frog.direction = _frogData.direction.ReturnV2FromUnSignedAngle();
        }

        butterFlySet.ClearSet();
        frogSet.ClearSet();
        occupyingEntitySet.ClearSet();

        foreach (var butterFly in _butterFlies)
        {
            butterFly.ResetToDefault();
            butterFly.SetData();
        }
        foreach (var frog in _frogs)
        {
            frog.ResetToDefault();
            frog.SetData();
        }

        SetUpLevel();
    }
}
