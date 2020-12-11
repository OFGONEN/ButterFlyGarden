using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class LevelManager : MonoBehaviour
{
    public Camera mainCamera;
    public LevelCreationSettings creationSettings;
    public LevelData levelData;
    public PlatformEntitySet platformEntitySet;
    public Transform parentTransform;

    private void Start()
    {
        CreateLevel();
        SetUpLevel();
    }
    public void CreateLevel()
    {
        for (int i = 0; i < levelData.lilyDatas.Count; i++)
        {
            var _lilyData = levelData.lilyDatas[i];

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

        for (int i = 0; i < levelData.butterFlyDatas.Count; i++)
        {
            var _butterFlyData = levelData.butterFlyDatas[i];

            var _butterFly = GameObject.Instantiate(_butterFlyData.levelObject);
            _butterFly.transform.SetParent(parentTransform);

            _butterFly.transform.position = new Vector3(_butterFlyData.position.x * creationSettings.horizontalDistance,
             creationSettings.butterFlyDistanceToLily, // Stands on top of lily
             _butterFlyData.position.y * creationSettings.verticalDistance);

            _butterFly.transform.rotation = Quaternion.Euler(0, _butterFlyData.direction, 0);

            var _butterflyComponent = _butterFly.GetComponent<ButterFly>();
            _butterflyComponent.color = _butterFlyData.butterFlyColor;
            _butterflyComponent.mapCord = _butterFlyData.mapCord;
            _butterflyComponent.direction = _butterFlyData.direction.ReturnV2FromUnSignedAngle();
            _butterflyComponent.SetData();
        }

        for (int i = 0; i < levelData._frogDatas.Count; i++)
        {
            var _frogData = levelData._frogDatas[i];

            var _frog = GameObject.Instantiate(_frogData.levelObject);
            _frog.transform.SetParent(parentTransform);

            _frog.transform.position = new Vector3(_frogData.position.x * creationSettings.horizontalDistance,
             creationSettings.frogFlyDistanceToLily, // Stands on top of lily
             _frogData.position.y * creationSettings.verticalDistance);

            _frog.transform.rotation = Quaternion.Euler(0, _frogData.direction, 0);

            var _frogComponent = _frog.GetComponent<Frog>();
            _frogComponent.color = _frogData.frogColor;
            _frogComponent.mapCord = _frogData.mapCord;
            _frogComponent.direction = _frogData.direction.ReturnV2FromUnSignedAngle();
            _frogComponent.SetData();
            Debug.Log(_frogComponent.direction + "  " + _frogData.direction);
        }

        parentTransform.position = new Vector3(-creationSettings.horizontalDistance * (levelData.size.x - 1) / 2,
        0,
        -creationSettings.verticalDistance * (levelData.size.y - 1) / 2);

        var _cameraPos = mainCamera.transform.position;
        _cameraPos.y = levelData.size.x * 3;
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
}
