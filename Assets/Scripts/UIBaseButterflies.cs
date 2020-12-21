using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class UIBaseButterflies : UIPingPong
{
    public EventListenerDelegateResponse recipeButterflyCrossEventListener;
    public EventListenerDelegateResponse levelRestartEventListener;
    public CurrentLevelData currentLevelData;
    public UIBaseButterfly[] baseButterflies;

    private void OnEnable()
    {
        recipeButterflyCrossEventListener.OnEnable();
        levelRestartEventListener.OnEnable();
    }
    private void OnDisable()
    {

        recipeButterflyCrossEventListener.OnDisable();
        levelRestartEventListener.OnDisable();
    }

    private void Start()
    {
        position = uiRectTransform.anchoredPosition;

        if (safeArea)
            destination += new Vector2(0, Mathf.Sign(destination.y) * (Screen.height - Screen.safeArea.height - Screen.safeArea.position.y));

        recipeButterflyCrossEventListener.response = RecipeButterflyEventResponse;
        levelRestartEventListener.response = SetData;
    }

    public void SetData()
    {
        ClearAllButterflies();

        var _butterflyDatas = currentLevelData.levelData.butterFlyDatas;

        int _baseButterflyIndex = 0;

        for (int i = 0; i < _butterflyDatas.Count; i++)
        {
            var _butterFly = _butterflyDatas[i];

            if (_butterFly.butterflyInRecipe)
            {
                var _color = _butterFly.butterFlyColor;
                _color.a = 1;

                baseButterflies[_baseButterflyIndex].butterflyWings.color = _color;
                baseButterflies[_baseButterflyIndex].gameObject.SetActive(true);
                _baseButterflyIndex++;
            }
        }
    }
    void RecipeButterflyEventResponse()
    {
        CrossButterfly((recipeButterflyCrossEventListener.gameEvent as IntGameEvent).intValue);
    }
    public void CrossButterfly(int index)
    {
        baseButterflies[index].CrossSequence();
    }
    public void ClearAllButterflies()
    {
        for (int i = 0; i < baseButterflies.Length; i++)
        {
            baseButterflies[i].Clear();
        }
    }
}
