using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformEntity : Entity
{
    public OccupyingEntity occupingEntity;
    [HideInInspector]
    public OccupyingEntity inComingEntity;

    [HideInInspector]
    public PlatformEntity upPlatformEntity;
    [HideInInspector]
    public PlatformEntity rightPlatformEntity;
    [HideInInspector]
    public PlatformEntity downPlatformEntity;
    [HideInInspector]
    public PlatformEntity leftPlatformEntity;

    public PlatformEntitySet platformEntitySet;
    public OccupyingEntitySet occupyingEntitySet;

    public PlatformEntity GetNeighborPlatform(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return upPlatformEntity;
        }
        else if (direction == Vector2.right)
        {
            return rightPlatformEntity;
        }
        else if (direction == Vector2.down)
        {
            return downPlatformEntity;
        }
        else if (direction == Vector2.left)
        {
            return leftPlatformEntity;
        }
        else
        {
            return null;
        }
    }

    public void FindNeighbors()
    {
        platformEntitySet.itemDictionary.TryGetValue(mapCord + Vector2.up, out upPlatformEntity);
        platformEntitySet.itemDictionary.TryGetValue(mapCord + Vector2.right, out rightPlatformEntity);
        platformEntitySet.itemDictionary.TryGetValue(mapCord + Vector2.down, out downPlatformEntity);
        platformEntitySet.itemDictionary.TryGetValue(mapCord + Vector2.left, out leftPlatformEntity);
    }
    public void FindOccupyingEntity()
    {

        occupyingEntitySet.itemDictionary.TryGetValue(mapCord, out occupingEntity);

        if (occupingEntity != null)
        {
            occupingEntity.platformEntity = this;
            occupingEntity.graphicTransform.SetParent(graphicTransform);
        }
    }
}
