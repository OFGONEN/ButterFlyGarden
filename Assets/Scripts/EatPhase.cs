using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EatPhase", menuName = "FF/Data/Phase/EatPhase")]
public class EatPhase : GamePhase
{
    public FrogSet frogSet;

    public override void Execute()
    {
        for (int i = frogSet.itemList.Count - 1; i >= 0; i--)
        {
            frogSet.itemList[i].TryEat();
        }

        if (wait == 0)
            ChangeNext();
    }
}
