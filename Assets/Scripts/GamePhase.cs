using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePhase : ScriptableObject
{
    public GamePhase nextPhase;
    public GameEvent phaseEnded;
    public int wait = 0;

    public abstract void Execute();
    public void ChangeNext()
    {
        if (phaseEnded != null)
            phaseEnded.Raise();

        if (nextPhase != null)
            nextPhase.Execute();
    }
    public void AddWait()
    {
        wait++;
    }
    public void RemoveWait()
    {
        wait--;
        if (wait <= 0)
        {
            wait = 0;
            ChangeNext();
        }
    }
    public void Reset()
    {
        wait = 0;
    }
}
