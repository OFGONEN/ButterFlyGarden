using System.Collections;
using System.Collections.Generic;
using FFStudio;
using UnityEngine;
using DG.Tweening;

public class GameLoopManager : MonoBehaviour
{
    public GamePhase entryPhase;
    public EventListenerDelegateResponse swipeInputEventListener;
    public EventListenerDelegateResponse endPhaseEventListener;
    public EventListenerDelegateResponse replayUIEventListener;
    public EventListenerDelegateResponse startLevelEventListener;
    public GameEvent targetButterflyAcquired;
    public GameEvent restartLevelEvent;
    public PlatformEntitySet platformEntitySet;
    public ButterFlySet butterFlySet;
    public MergedButterFlySet mergedButterFlySet;
    public FrogSet frogSet;
    public NewCreatedObjectsSet newCreatedObjects;
    public CurrentLevelData currentLevelData;

    public bool gameLoopStarted = true;
    public List<int> acquiredTargets = new List<int>(4);
    private void OnEnable()
    {
        swipeInputEventListener.OnEnable();
        replayUIEventListener.OnEnable();
        endPhaseEventListener.OnEnable();
        startLevelEventListener.OnEnable();
    }

    private void OnDisable()
    {
        swipeInputEventListener.OnDisable();
        replayUIEventListener.OnDisable();
        endPhaseEventListener.OnDisable();
        startLevelEventListener.OnDisable();
    }
    private void Start()
    {
        swipeInputEventListener.response = SwipeInputResponse;
        replayUIEventListener.response = ReplayUIResponse;
        endPhaseEventListener.response = EndLoopCheck;
        startLevelEventListener.response = StartLevelResponse;
    }
    private void StartLevelResponse()
    {
        gameLoopStarted = false;
    }
    private void LevelLoadedResponse()
    {
        acquiredTargets.Clear();
    }
    private void ReplayUIResponse()
    {
        if (gameLoopStarted) return;


        DOTween.KillAll();

        for (int i = newCreatedObjects.itemList.Count - 1; i >= 0; i--)
        {
            newCreatedObjects.itemList[i].graphicTransform.SetParent(newCreatedObjects.itemList[i].transform);
            Destroy(newCreatedObjects.itemList[i].gameObject);
        }

        newCreatedObjects.itemList.Clear();
        newCreatedObjects.itemDictionary.Clear();
        acquiredTargets.Clear();

        restartLevelEvent.Raise();
    }
    private void SwipeInputResponse()
    {
        var _swipeInputEvent = swipeInputEventListener.gameEvent as SwipeInputEvent;

        if (gameLoopStarted || _swipeInputEvent.swipeDirection == Vector2.zero) return;

        gameLoopStarted = true;
        entryPhase.Execute();
    }

    public void EndLoopCheck()
    {
        gameLoopStarted = false;

        for (int i = 0; i < mergedButterFlySet.itemList.Count; i++)
        {
            AcquireTarget(mergedButterFlySet.itemList[i]);
        }

        if (acquiredTargets.Count == currentLevelData.levelData.targetButterFlyDatas.Count)
        {
            gameLoopStarted = true;
        }
    }
    public void AcquireTarget(MergedButterFly mergedButterFly)
    {
        var _instanceId = mergedButterFly.patternTexture.GetInstanceID();
        var _levelData = currentLevelData.levelData;

        for (int i = 0; i < _levelData.targetButterFlyDatas.Count; i++)
        {
            var _finalPatternInstanceId = _levelData.targetButterFlyDatas[i].finalPattern.GetInstanceID();
            var _samePattern = _instanceId == _finalPatternInstanceId;
            var _matchColors = MatchColors(_levelData.targetButterFlyDatas[i], mergedButterFly);

            if (_samePattern && _matchColors && !acquiredTargets.Contains(i))
            {
                acquiredTargets.Add(i);
                PopAcquireTarget(i, mergedButterFly);
            }
        }
    }
    public bool MatchColors(TargetButterFlyData targetButterFlyData, MergedButterFly mergedButterFly)
    {
        if (targetButterFlyData.butterFlyColors.Count != mergedButterFly.inputButterFlies.Count) return false;

        bool _sameColor = true;
        for (int i = 0; i < targetButterFlyData.butterFlyColors.Count; i++)
        {
            var _butterFlyColor = mergedButterFly.inputButterFlies[i].color;
            _sameColor &= targetButterFlyData.butterFlyColors.FindSameColor(_butterFlyColor);
        }

        return _sameColor;
    }

    public void PopAcquireTarget(int index, MergedButterFly acquiredTarget)
    {
        if (acquiredTarget.attachedEntity is Bubble)
            (acquiredTarget.attachedEntity as Bubble).Pop();

        acquiredTarget.entityAnimator.SetTrigger("Fly");

        acquiredTarget.graphicTransform.SetParent(acquiredTarget.transform);
        acquiredTarget.transform.DOMove(new Vector3(0, currentLevelData.mainCameraPos.y - 3f, -1f), 1).OnComplete(() =>
           {
               acquiredTarget.entityAnimator.SetTrigger("Idle");
               acquiredTarget.entityAnimator.SetFloat("IdleBlend", 1);
               targetButterflyAcquired.Raise();
           });
        acquiredTarget.transform.DORotate(new Vector3(0, 0, 0), 1);
    }
}
