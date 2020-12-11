using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Camera mainCamera;
    public LevelCreationSettings creationSettings;
    public LevelData levelData;
    public Transform parentTransform;

    private void Start()
    {
        CreateLevel();
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
            _butterFly.GetComponent<ButterFly>().color = _butterFlyData.butterFlyColor;
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
        }

        parentTransform.position = new Vector3(-creationSettings.horizontalDistance * (levelData.size.x - 1) / 2,
        0,
        -creationSettings.verticalDistance * (levelData.size.y - 1) / 2);


        var _cameraPos = mainCamera.transform.position;
        _cameraPos.y = levelData.size.x * 3;
        mainCamera.transform.position = _cameraPos;
    }
}
