using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementPhase", menuName = "FF/Data/Phase/MovementPhase")]
public class MovementPhase : GamePhase
{
    public ButterFlySet butterFlySet;
    public SwipeInputEvent swipeInputEvent;
    public override void AddWait()
    {
        wait++;
    }

    public override void ChangeNext()
    {
        if (phaseEnded != null)
            phaseEnded.Raise();

        if (nextPhase != null)
            nextPhase.Execute();
    }
    public override void Execute()
    {
        for (int i = butterFlySet.itemList.Count - 1; i >= 0; i--)
        {
            butterFlySet.itemList[i].MoveToPlatform(swipeInputEvent.swipeDirection);
        }

        if (wait == 0)
            ChangeNext();
    }

    public override void RemoveWait()
    {
        wait--;
        if (wait <= 0)
        {
            wait = 0;
            ChangeNext();
        }
    }

    public override void Reset()
    {
        wait = 0;
    }
}
