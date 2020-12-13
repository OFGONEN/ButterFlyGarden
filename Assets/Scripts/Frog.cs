using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : OccupyingEntity
{
    public Color color;

    public OccupyingEntitySet occupyingEntitySet;
    public FrogSet frogSet;

    [HideInInspector]
    public FrogData frogData;
    private void OnEnable()
    {
        if (!hasData) return;

        occupyingEntitySet.AddDictionary(mapCord, this);
        occupyingEntitySet.AddList(this);

        frogSet.AddDictionary(mapCord, this);
        frogSet.AddList(this);
    }

    private void Start()
    {
        renderer.material.color = color;
    }
    private void OnDisable()
    {
        occupyingEntitySet.RemoveList(this);
        occupyingEntitySet.RemoveDictionary(mapCord);

        frogSet.RemoveList(this);
        frogSet.RemoveDictionary(mapCord);
    }

    public void Eat()
    {
        var _platform = platformEntity.GetNeighborPlatform(direction);

        if (_platform != null && _platform.occupingEntity != null && _platform.occupingEntity is ButterFly)
        {
            _platform.occupingEntity.gameObject.SetActive(false);
            _platform.occupingEntity = null;
        }
    }

    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }

    public override void ResetToDefault()
    {
        // has a cooldown etc.
    }
}
