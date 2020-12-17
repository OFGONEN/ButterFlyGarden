using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AppManager : MonoBehaviour
{
    public GameEvent nextLevelLoadedEvent;
    public EventListenerDelegateResponse loadNextLevelEventListener;
    public CurrentLevelData currentLevel;
    private void OnEnable()
    {
        loadNextLevelEventListener.OnEnable();
    }
    private void OnDisable()
    {
        loadNextLevelEventListener.OnDisable();
    }
    private void Start()
    {
        currentLevel.currentLevel = PlayerPrefs.GetInt("Level", 1);
        currentLevel.LoadCurrentLevelData();

        loadNextLevelEventListener.response = LoadLevel;
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
        _operation.completed += UnloadPreviousSceneDone;
    }
    void UnloadPreviousSceneDone(AsyncOperation operation)
    {
        var _operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        _operation.completed += NextLevelLoaded;
    }

    void NextLevelLoaded(AsyncOperation operation)
    {
        StartCoroutine(RaiseNextLevelLoadedEvent());
    }

    IEnumerator RaiseNextLevelLoadedEvent()
    {
        yield return new WaitForEndOfFrame();
        nextLevelLoadedEvent.Raise();
    }

}
