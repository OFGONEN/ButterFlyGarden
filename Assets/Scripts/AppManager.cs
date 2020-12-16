using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AppManager : MonoBehaviour
{
    public CurrentLevelData currentLevel;
    public EventListenerDelegateResponse targetAquired;

    private void OnEnable()
    {
        targetAquired.OnEnable();
    }
    private void OnDisable()
    {
        targetAquired.OnDisable();
    }
    private void Start()
    {
        targetAquired.response = LoadLevel;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    void LoadLevel()
    {
        DOTween.KillAll();

        currentLevel.levelData = null;
        Resources.UnloadUnusedAssets();

        currentLevel.currentLevel++;
        currentLevel.LoadCurrentLevelData();

        var _operation = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(1));
        _operation.completed += LoadNextLevel;
    }

    void LoadNextLevel(AsyncOperation operation)
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
}
