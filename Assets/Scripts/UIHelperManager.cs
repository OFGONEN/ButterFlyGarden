using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelperManager : MonoBehaviour
{
    public EventListenerDelegateResponse levelStartEventListener;
    public EventListenerDelegateResponse levelRestartEventListener;
    public CurrentLevelData currentLevelData;

    public GameObject[] helperAssets;
    public Dictionary<string, UIHelperAsset> helperAssetDictionary = new Dictionary<string, UIHelperAsset>(8);

    public UITutorial currentUITutorial;

    private void Awake()
    {
        for (int i = 0; i < helperAssets.Length; i++)
        {
            var _helperUIComponent = helperAssets[i].GetComponent<UIHelperAsset>();
            helperAssetDictionary.Add(helperAssets[i].gameObject.name, _helperUIComponent);
        }
    }
    private void OnEnable()
    {
        levelStartEventListener.OnEnable();
        levelRestartEventListener.OnEnable();
    }
    private void OnDisable()
    {
        levelStartEventListener.OnDisable();
        levelRestartEventListener.OnDisable();
    }
    private void Start()
    {
        levelStartEventListener.response = StartLevelResponse;
        levelRestartEventListener.response = RestartLevelResponse;
    }
    void StartLevelResponse()
    {
        if (currentLevelData.levelData.uiTutorial != null)
        {
            StartTutorial(currentLevelData.levelData.uiTutorial);
        }
    }
    void RestartLevelResponse()
    {
        if (currentUITutorial != null)
            currentUITutorial.ForseEndTutorial();

        if (currentLevelData.levelData.uiTutorial != null)
        {
            StartTutorial(currentLevelData.levelData.uiTutorial);
        }
    }
    void StartTutorial(UITutorial uiTutorial)
    {
        for (int i = 0; i < helperAssets.Length; i++)
        {
            helperAssets[i].gameObject.SetActive(false);
        }

        currentUITutorial = uiTutorial;
        currentUITutorial.tutorialEnded = TutorialEnded;
        currentUITutorial.uIHelperManager = this;

        currentUITutorial.StartTutorial();
    }

    void TutorialEnded(bool success)
    {
        if (success && currentUITutorial.nextTutorial != null)
        {
            StartTutorial(currentUITutorial.nextTutorial);
        }
    }
}
