﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Frog : OccupyingEntity
{
    public ParticleSystem particleSystem;
    public Color color;

    public EatPhase eatPhase;
    public OccupyingEntitySet occupyingEntitySet;
    public FrogSet frogSet;
    public LevelCreationSettings levelCreationSettings;

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

    public void TryEat()
    {
        var _platform = platformEntity.GetNeighborPlatform(direction);

        if (_platform != null && _platform.occupingEntity != null && _platform.occupingEntity is ButterFly)
        {
            eatPhase.AddWait();

            entityAnimator.SetTrigger("Attack");
            // entityAnimator.SetBool("Chew", true);

            var _targetTransform = _platform.occupingEntity.transform;

            _targetTransform.DOMove(transform.position, levelCreationSettings.butterFlyFlyDuration).OnComplete(() => Eat(_platform));
            _targetTransform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), levelCreationSettings.butterFlyFlyDuration);

        }
    }
    void Eat(PlatformEntity platform)
    {
        particleSystem.Play();
        platformEntity.entityAnimator.SetTrigger("Ripple");
        platform.occupingEntity.gameObject.SetActive(false);
        platform.occupingEntity = null;
    }
    void EndChew()
    {
        eatPhase.RemoveWait();
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
