using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction : ScriptableObject
{
    public abstract void Execute(OccupyingEntity actionCaller);
}
