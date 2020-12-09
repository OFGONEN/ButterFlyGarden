using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventListener
{
    public GameEvent gameEvent;
    public abstract void OnEnable();
    public abstract void OnDisable();

    public abstract void OnEventRaised();
}
