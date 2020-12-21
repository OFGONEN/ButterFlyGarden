using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public CurrentLevelData currentLevelData;
    public GameEvent startLevelEvent;
    public GameEvent loadNextLevelEvent;
    public EventListenerDelegateResponse tapInputEventListener;
    public EventListenerDelegateResponse targetAquiredEventListener;
    public EventListenerDelegateResponse nextLevelLoadedEventListener;
    public Image backGroundImage;
    public Image butterFlyImage;
    public UIBaseButterflies targetButterflies;
    public UIPingPong introductionText;
    public UIPingPong resetButton;
    public UIPingPong levelText;
    public UIPingPong tapToPlayButton;
    public RectTransform leftSide;
    public RectTransform center;
    public RectTransform rightSide;

    private void OnEnable()
    {
        tapInputEventListener.OnEnable();
        targetAquiredEventListener.OnEnable();
        nextLevelLoadedEventListener.OnEnable();
    }
    private void OnDisable()
    {
        tapInputEventListener.OnDisable();
        targetAquiredEventListener.OnDisable();
        nextLevelLoadedEventListener.OnDisable();
    }
    void Start()
    {
        backGroundImage.DOFade(0.8f, 0.5f);
        butterFlyImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;
        tapInputEventListener.response = TapToPlayPressed;
        targetAquiredEventListener.response = TargetAquired;
        nextLevelLoadedEventListener.response = NewLevelLoaded;
    }

    void TapToPlayPressed()
    {
        targetButterflies.SetData();
        levelText.uiText.text = $"Level {currentLevelData.currentLevel}";
        tapInputEventListener.response = EmptyMethod;

        if (currentLevelData.levelData.levelIntroductionData.introduce)
        {
            butterFlyImage.rectTransform.DOMove(leftSide.transform.position, 0.5f).OnComplete(IntroduceButterFly);
            introductionText.GoDestination();
            tapToPlayButton.GoDestination();
        }
        else
        {
            StartLevelAfterIntroduce();
        }
    }
    void IntroduceButterFly()
    {
        backGroundImage.DOFade(0.8f, 0.5f).OnComplete(() => tapInputEventListener.response = StartLevelAfterIntroduce);
        introductionText.uiText.text = currentLevelData.levelData.levelIntroductionData.introductionText;
        butterFlyImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;
        butterFlyImage.transform.position = rightSide.position;
        tapToPlayButton.uiText.text = "Tap to Start!";

        butterFlyImage.enabled = true;
        butterFlyImage.rectTransform.DOMove(center.transform.position, 0.5f);
        introductionText.GoDestination();
        tapToPlayButton.GoDestination();
    }

    void StartLevelAfterIntroduce()
    {
        tapInputEventListener.response = EmptyMethod;

        backGroundImage.DOFade(0, 0.5f);
        targetButterflies.GoDestination();
        levelText.GoDestination();
        resetButton.GoDestination();
        tapToPlayButton.GoDestination();
        introductionText.GoDestination();

        butterFlyImage.rectTransform.DOMove(leftSide.transform.position, 0.25f);
        startLevelEvent.Raise();
    }

    void LoadNextLevel()
    {
        tapInputEventListener.response = EmptyMethod;
        backGroundImage.DOFade(1, 0.5f).OnComplete(() => loadNextLevelEvent.Raise());
        introductionText.GoDestination();
        tapToPlayButton.GoDestination();
    }

    void TargetAquired()
    {
        introductionText.uiText.text = currentLevelData.levelData.levelIntroductionData.levelWinText;
        tapToPlayButton.uiText.text = "Next Level!";
        PlayerPrefs.SetInt("Level", currentLevelData.currentLevel + 1);

        introductionText.GoDestination();
        targetButterflies.GoDestination();
        levelText.GoDestination();
        resetButton.GoDestination();
        tapToPlayButton.GoDestination();

        tapInputEventListener.response = LoadNextLevel;
    }

    void NewLevelLoaded()
    {
        targetButterflies.SetData();
        levelText.uiText.text = $"Level {currentLevelData.currentLevel}";
        backGroundImage.DOFade(0, 0.2f);

        if (currentLevelData.levelData.levelIntroductionData.introduce)
        {
            IntroduceButterFly();
        }
        else
        {
            StartLevel();
        }
    }
    void StartLevel()
    {
        tapInputEventListener.response = EmptyMethod;
        startLevelEvent.Raise();

        targetButterflies.GoDestination();
        resetButton.GoDestination();
        levelText.GoDestination();
    }

    void EmptyMethod()
    {

    }
}
