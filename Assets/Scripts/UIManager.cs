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
    public EventListenerDelegateResponse tapToPlayEventListener;
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
    }
    private void OnDisable()
    {
        tapToPlayEventListener.OnDisable();
    }
    void Start()
    {
        tapToPlayEventListener.response = TapToPlayPressed;
    }

    void TapToPlayPressed()
    {
        backGroundImage.DOFade(0, 0.5f);
        targetImage.targetImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;

        if (currentLevelData.levelData.levelIntroductionData.introduce)
        {
            StartLevel();
        }
        else
        {
            butterFlyImage.rectTransform.DOMove(leftSide.transform.position, 0.5f).OnComplete(IntroduceButterFly);
        }
    }
    void IntroduceButterFly()
    {
        introductionText.uiText.text = currentLevelData.levelData.levelIntroductionData.introductionText;
        butterFlyImage.sprite = currentLevelData.levelData.levelIntroductionData.targetButterFlyImage;

        butterFlyImage.transform.position = rightSide.position;
        butterFlyImage.rectTransform.DOMove(center.transform.position, 0.5f);

        tapToPlayEventListener.response = StartLevel;
    }

    void StartLevel()
    {
        targetImage.GoDestination();
        resetButton.GoDestination();
        tapToPlayButton.GoDestination();
        butterFlyImage.rectTransform.DOMove(leftSide.transform.position, 0.25f);
        introductionText.GoDestination();
        startLevelEvent.Raise();
    }
}
