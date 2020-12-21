using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "SwipeTutorial", menuName = "FF/Tutorial/SwipeTutorial")]
public class UISwipeTutorial : UITutorial
{
    public EventListenerDelegateResponse swipeInputEventListener;
    public UIRightSwipeTutorialData tutorialData;
    UIHelperAsset hand;
    UIHelperAsset arrowLine;
    // UIHelperAsset tutorialText;

    bool responsedInput = false;
    int tweenStarted = 0;
    public override void StartTutorial()
    {
        tweenStarted = 0;
        responsedInput = false;

        hand = GetHelperAsset(tutorialData.handAssetName);
        arrowLine = GetHelperAsset(tutorialData.arrowLineAssetName);
        // tutorialText = GetHelperAsset(tutorialData.textMessageAssetName);

        swipeInputEventListener.OnEnable();
        swipeInputEventListener.response = SwipeInputResponse;

        SetHelperAssetDatas();

        hand.uiRectTransform.DOAnchorPos(hand.uiRectTransform.anchoredPosition + tutorialData.handEndAttive, 1f).SetLoops(-1);
    }
    public override void SetHelperAssetDatas()
    {
        arrowLine.ResetAsset();
        arrowLine.uiRectTransform.anchoredPosition = tutorialData.arrowPosition;
        arrowLine.gameObject.SetActive(true);

        hand.ResetAsset();
        hand.uiRectTransform.anchoredPosition = tutorialData.handStartPosition;
        hand.gameObject.SetActive(true);

        // tutorialText.ResetAsset();
        // tutorialText.uiRectTransform.anchoredPosition = tutorialData.textPosition;
        // tutorialText.uiText.text = tutorialData.textMessage;
        // tutorialText.gameObject.SetActive(true);
    }
    public override void EndTutorial(bool success)
    {
        tweenStarted--;

        if (tweenStarted > 0)
            return;

        hand.uiImage.DOKill();
        arrowLine.uiImage.DOKill();
        // tutorialText.uiText.DOKill();

        tweenStarted = 0;
        responsedInput = false;

        swipeInputEventListener.OnDisable();
        tutorialEnded(success);
    }
    public override void ForseEndTutorial()
    {
        hand.uiRectTransform.DOKill();
        hand.uiImage.DOKill();
        arrowLine.uiImage.DOKill();
        // tutorialText.uiText.DOKill();


        tweenStarted = 0;
        responsedInput = false;
        swipeInputEventListener.OnDisable();
    }
    public override UIHelperAsset GetHelperAsset(string assetName)
    {
        UIHelperAsset _asset;
        uIHelperManager.helperAssetDictionary.TryGetValue(assetName, out _asset);

        return _asset;
    }
    public void SwipeInputResponse()
    {
        if (responsedInput) return;

        responsedInput = true;

        var _input = (swipeInputEventListener.gameEvent as SwipeInputEvent).swipeDirection;

        if (_input == tutorialData.expectedInput)
        {
            //correct
            GoColor(Color.green, true);
        }
        else if (_input == tutorialData.notrInput)
        {
            GoColor(Color.white, false);
        }
        else
        {
            //invalid
            GoColor(Color.red, false);
        }
    }
    public void GoColor(Color color, bool success)
    {
        hand.uiRectTransform.DOKill();

        hand.uiImage.color = color;
        tweenStarted++;
        hand.uiImage.DOFade(0f, 1f).SetEase(Ease.InQuint).OnComplete(() => EndTutorial(success));

        arrowLine.uiImage.color = color;
        tweenStarted++;
        arrowLine.uiImage.DOFade(0f, 1f).SetEase(Ease.InQuint).OnComplete(() => EndTutorial(success));


        // tutorialText.uiText.color = color;
        // tweenStarted++;
        // tutorialText.uiText.DOFade(0, 1f).SetEase(Ease.InQuint).OnComplete(() => EndTutorial(success));
    }
}

[System.Serializable]
public struct UIRightSwipeTutorialData
{
    public Vector2 handStartPosition;
    public Vector2 handEndAttive;
    public Vector2 arrowPosition;
    public Vector2 textPosition;
    public Vector2 expectedInput;
    public Vector2 notrInput;
    public string handAssetName;
    public string arrowLineAssetName;
    public string textMessageAssetName;
    public string textMessage;
}
