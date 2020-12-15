using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bubble : OccupyingEntity
{
    public Color color;
    public Vector3 rotateBy = new Vector3(1, 1, 1);
    public ParticleSystem particles;
    public OccupyingEntitySet occupyingEntitySet;
    public BubbleSet bubbleSet;

    [HideInInspector]
    public BubbleData bubbleData;


    private void OnEnable()
    {
        if (!hasData) return;


        occupyingEntitySet.AddDictionary(mapCord, this);
        occupyingEntitySet.AddList(this);

        bubbleSet.AddDictionary(mapCord, this);
        bubbleSet.AddList(this);
    }
    private void OnDisable()
    {
        occupyingEntitySet.RemoveDictionary(mapCord);
        occupyingEntitySet.RemoveList(this);

        bubbleSet.RemoveList(this);
        bubbleSet.RemoveDictionary(mapCord);
    }
    private void OnParticleSystemStopped()
    {
        // gameObject.SetActive(false);
    }
    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.

        transform.GetChild(0).DORotate(rotateBy, 1).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
    public override void ResetToDefault()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Pop()
    {
        if (attachedEntity != null)
            attachedEntity.attachedEntity = null;

        attachedEntity = null;
        transform.GetChild(0).gameObject.SetActive(false);
        particles.Play();
    }

    public void Attach(OccupyingEntity occupyingEntity)
    {
        if (attachedEntity != null)
            attachedEntity.attachedEntity = null;

        attachedEntity = occupyingEntity;
        attachedEntity.attachedEntity = this;
        transform.SetParent(occupyingEntity.transform);
    }
    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }
}
