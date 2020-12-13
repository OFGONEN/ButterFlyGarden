﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lily : PlatformEntity
{
    private void OnEnable()
    {
        if (!hasData) return;

        platformEntitySet.AddList(this);
        platformEntitySet.AddDictionary(mapCord, this);
    }

    public override void SetData()
    {
        hasData = true;

        OnEnable();
    }
    public override void ResetToDefault()
    {
        inComingEntity = null;
        occupingEntity = null;
    }
    private void OnDisable()
    {
        platformEntitySet.RemoveList(this);
        platformEntitySet.RemoveDictionary(mapCord);
    }
}
