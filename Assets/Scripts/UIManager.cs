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
    public EventListenerDelegateResponse tapToPlayEventListener;
    public EventListenerDelegateResponse targetAquiredEventListener;
    public EventListenerDelegateResponse nextLevelLoadedEventListener;
    public Image backGroundImage;
    public Image butterFlyImage;
    public UIPingPong targetImage;
    public UIPingPong introductionText;
    public UIPingPong resetButton;
    public UIPingPong tapToPlayButton;
    public RectTransform leftSide;
    public RectTransform center;
    public RectTransform rightSide;

    private void OnEnable()
    {
        tapToPlayEventListener.OnEnable();
        targetAquiredEventListener.OnEnable();
        nextLevelLoadedEventListener.OnEnable();
    }
    private void OnDisable()
    {
        tapToPlayEventListener.OnDisable();
        targetAquiredEventListener.OnDisable();
        nextLevelLoadedEventListener.OnDisable();
    }
    void Start()
    {
        tapToPlayEventListener.response = TapToPlayPressed;
        targetAquiredEventListener.response = TargetAquired;
        nextLevelLoadedEventListener.response = NewLevelLoaded;
    }

    void TapToPlayPressed()
    {
        backGroundImage.DOFade(0, 0.5f);
        targetImage.targetImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;

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
        introductionText.uiText.text = currentLevelData.levelData.levelIntroductionData.introductionText;
        butterFlyImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;
        butterFlyImage.transform.position = rightSide.position;
        tapToPlayButton.uiText.text = "Tap To Play";

        butterFlyImage.rectTransform.DOMove(center.transform.position, 0.5f);
        introductionText.GoDestination();
        tapToPlayButton.GoDestination();

        tapToPlayEventListener.response = StartLevelAfterIntroduce;
    }

    void StartLevelAfterIntroduce()
    {
        targetImage.GoDestination();
        resetButton.GoDestination();
        tapToPlayButton.GoDestination();
        introductionText.GoDestination();

        butterFlyImage.rectTransform.DOMove(leftSide.transform.position, 0.25f);
        startLevelEvent.Raise();
    }

    void LoadNextLevel()
    {
        backGroundImage.DOFade(1, 0.5f).OnComplete(() => loadNextLevelEvent.Raise());
        introductionText.GoDestination();
        tapToPlayButton.GoDestination();
    }

    void TargetAquired()
    {
        introductionText.uiText.text = currentLevelData.levelData.levelIntroductionData.levelWinText;
        tapToPlayButton.uiText.text = "Next Level";

        introductionText.GoDestination();
        targetImage.GoDestination();
        resetButton.GoDestination();
        tapToPlayButton.GoDestination();

        tapToPlayEventListener.response = LoadNextLevel;
    }

    void NewLevelLoaded()
    {
        targetImage.targetImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;
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
        targetImage.GoDestination();
        resetButton.GoDestination();
        startLevelEvent.Raise();
    }
}
