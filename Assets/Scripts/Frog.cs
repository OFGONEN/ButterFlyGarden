using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Frog : OccupyingEntity
{
    public ParticleSystem particles;
    public Color color;
    public EatPhase eatPhase;
    public OccupyingEntitySet occupyingEntitySet;
    public FrogSet frogSet;
    public LevelCreationSettings levelCreationSettings;

    [HideInInspector]
    public FrogData frogData;

    public bool chewing = false;
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
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.

    }
    private void OnDisable()
    {
        occupyingEntitySet.RemoveList(this);
        occupyingEntitySet.RemoveDictionary(frogData.mapCord);

        frogSet.RemoveList(this);
        frogSet.RemoveDictionary(frogData.mapCord);
    }

    public void TryEat()
    {
        var _platform = platformEntity.GetNeighborPlatform(direction);


        if (_platform != null && _platform.occupingEntity != null && _platform.occupingEntity is ButterFly)
        {
            var _butterFly = _platform.occupingEntity as ButterFly;

            if (_butterFly.attachedEntity != null && _butterFly.attachedEntity is Bubble)
            {
                chewing = false;

                var _bubble = _butterFly.attachedEntity as Bubble;
                _bubble.Pop();
            }
            else
            {
                chewing = true;

                var _targetTransform = _platform.occupingEntity.transform;
                _platform.occupingEntity.graphicTransform.SetParent(_targetTransform);

                _targetTransform.DOMove(transform.position, levelCreationSettings.butterFlyFlyDuration).OnComplete(() => Eat(_platform));
                _targetTransform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), levelCreationSettings.butterFlyFlyDuration);

            }

            eatPhase.AddWait();
            entityAnimator.SetTrigger("Attack");
            entityAnimator.SetBool("Chew", chewing);
        }
    }
    void PlaySound(SoundEvent soundEvent)
    {
        soundEvent.Raise();
    }
    void Eat(PlatformEntity platform)
    {
        // particles.Play();
        platformEntity.entityAnimator.SetTrigger("Ripple");
        platform.occupingEntity.gameObject.SetActive(false);
        platform.occupingEntity = null;
    }
    void EndAttack()
    {
        if (!chewing)
            eatPhase.RemoveWait();
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
