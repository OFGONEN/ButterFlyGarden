using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EncounterPhase", menuName = "FF/Data/Phase/EncounterPhase")]
public class EncounterPhase : GamePhase
{
    public ButterFlySet butterFlySet;

    public override void Execute()
    {
        for (int i = butterFlySet.itemList.Count - 1; i >= 0; i--)
        {
            butterFlySet.itemList[i].Encounter();
        }

        if (wait == 0)
            ChangeNext();
    }
}
