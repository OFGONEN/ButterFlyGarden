using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePhase : ScriptableObject
{
    public GamePhase nextPhase;
    public GameEvent phaseEnded;
    [HideInInspector]
    public int wait = 0;

    public abstract void Execute();
    public abstract void ChangeNext();
    public abstract void AddWait();
    public abstract void RemoveWait();
    public abstract void Reset();
}
