using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "FF/Event/GameEvent")]
public class GameEvent : ScriptableObject
{

    private readonly List<EventListener> eventListeners =
        new List<EventListener>();

    public void Raise()
    {

        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised();
    }

    public void RegisterListener(EventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(EventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
