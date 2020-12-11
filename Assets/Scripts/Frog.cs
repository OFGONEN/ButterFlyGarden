using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : OccupyingEntity
{
    public Color color;

    public OccupyingEntitySet occupyingEntitySet;
    public FrogSet frogSet;

    private void OnEnable()
    {
        if (!hasData) return;

        occupyingEntitySet.itemDictionary.Add(mapCord, this);
        occupyingEntitySet.itemList.Add(this);

        frogSet.itemDictionary.Add(mapCord, this);
        frogSet.itemList.Add(this);
    }


    private void Start()
    {
        renderer.material.color = color;
    }
    private void OnDisable()
    {
        occupyingEntitySet.itemList.Remove(this);
        occupyingEntitySet.itemDictionary.Remove(mapCord);

        frogSet.itemList.Remove(this);
        frogSet.itemDictionary.Remove(mapCord);
    }

    public void Eat()
    {
        var _platform = platformEntity.GetNeighborPlatform(direction);

        if (_platform != null && _platform.occupingEntity != null && _platform.occupingEntity is ButterFly)
        {
            Destroy(_platform.occupingEntity.gameObject);
            _platform.occupingEntity = null;
        }
    }

    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }
}
