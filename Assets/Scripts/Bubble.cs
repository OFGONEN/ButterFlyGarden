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
    public SoundEvent bubblePopSound;

    [HideInInspector]
    public BubbleData bubbleData;
    Transform levelTransform;
    Tween verticalTween;


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
        occupyingEntitySet.RemoveDictionary(bubbleData.mapCord);
        occupyingEntitySet.RemoveList(this);

        bubbleSet.RemoveList(this);
        bubbleSet.RemoveDictionary(bubbleData.mapCord);
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

        levelTransform = transform.parent;

        graphicTransform.DORotate(rotateBy, 1).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        verticalTween = graphicTransform.DOMoveY(0.25f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
    public override void ResetToDefault()
    {
        graphicTransform.gameObject.SetActive(true);

        if (attachedEntity != null)
            attachedEntity.attachedEntity = null;

        attachedEntity = null;

        graphicTransform.DORotate(rotateBy, 1).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        verticalTween = graphicTransform.DOMoveY(0.25f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
    public void Pop()
    {
        if (attachedEntity != null)
            attachedEntity.attachedEntity = null;

        attachedEntity = null;


        graphicTransform.SetParent(transform);
        graphicTransform.gameObject.SetActive(false);
        graphicTransform.localPosition = Vector3.zero;
        transform.SetParent(levelTransform);

        particles.Play();
        bubblePopSound.Raise();
    }

    public void Attach(OccupyingEntity occupyingEntity)
    {
        if (attachedEntity != null)
            attachedEntity.attachedEntity = null;

        verticalTween.Kill();

        attachedEntity = occupyingEntity;
        attachedEntity.attachedEntity = this;
        graphicTransform.SetParent(transform);
        graphicTransform.localPosition = Vector3.zero;
        transform.SetParent(occupyingEntity.graphicTransform);
    }
    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }
}
