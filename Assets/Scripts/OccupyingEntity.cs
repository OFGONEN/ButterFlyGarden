using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OccupyingEntity : Entity
{
    public PlatformEntity platformEntity;

    public List<GameAction> movementActions = new List<GameAction>();
    public List<GameAction> encounterActions = new List<GameAction>();

    public void RunActions(List<GameAction> gameActions)
    {
        for (int i = 0; i < gameActions.Count; i++)
        {
            gameActions[i].Execute(this);
        }
    }
}
