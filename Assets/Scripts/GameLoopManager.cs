using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public EventListenerDelegateResponse swipeInputEventListener;
    public PlatformEntitySet platformEntitySet;
    public ButterFlySet butterFlySet;
    public FrogSet frogSet;


    private void OnEnable()
    {
        swipeInputEventListener.OnEnable();
    }

    private void OnDisable()
    {
        swipeInputEventListener.OnDisable();
    }
    private void Start()
    {
        swipeInputEventListener.response = SwipeInputResponse;
    }

    private void SwipeInputResponse()
    {
        var _swipeInputEvent = swipeInputEventListener.gameEvent as SwipeInputEvent;

        if (_swipeInputEvent.swipeDirection == Vector2.zero) return;


        for (int i = butterFlySet.itemList.Count - 1; i >= 0; i--)
        {
            var _moved = butterFlySet.itemList[i].MoveToPlatform(_swipeInputEvent.swipeDirection);
            if (_moved) butterFlySet.itemList[i].Encounter();
        }

        for (int i = frogSet.itemList.Count - 1; i >= 0; i--)
        {
            frogSet.itemList[i].Eat();
        }
    }

}
