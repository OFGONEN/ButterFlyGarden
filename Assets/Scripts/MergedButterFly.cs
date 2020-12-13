using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergedButterFly : ButterFly
{
    public MergedButterFlySet mergedButterFlySet;
    public NewCreatedObjectsSet newCreatedObjectSet;
    public Texture2D patternTexture;

    public List<ButterFly> inputButterFlies;

    private static readonly int inputButterflyColorsShaderID = Shader.PropertyToID("inputButterflyColors");
    private static readonly int numberOfInputButterfliesShaderID = Shader.PropertyToID("numberOfInputButterflies");

    private void OnEnable()
    {
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetTexture("_MainTex", patternTexture);

        // Array sizes are determined the first time they are set, so set it to max beforehand.
        materialPropertyBlock.SetFloatArray(inputButterflyColorsShaderID, new float[3 * 10]);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.

        if (!hasData) return;

        occupyingEntitySet.AddDictionary(mapCord, this);
        occupyingEntitySet.AddList(this);

        butterFlySet.AddDictionary(mapCord, this);
        butterFlySet.AddList(this);

        mergedButterFlySet.AddDictionary(mapCord, this);
        mergedButterFlySet.AddList(this);

        newCreatedObjectSet.AddDictionary(mapCord, gameObject);
        newCreatedObjectSet.AddList(gameObject);
    }
    private void OnDisable()
    {
        occupyingEntitySet.RemoveList(this);
        occupyingEntitySet.RemoveDictionary(mapCord);

        butterFlySet.RemoveList(this);
        butterFlySet.RemoveDictionary(mapCord);

        mergedButterFlySet.RemoveList(this);
        mergedButterFlySet.RemoveDictionary(mapCord);
    }
    private void OnDestroy()
    {
        newCreatedObjectSet.RemoveDictionary(mapCord);
        newCreatedObjectSet.RemoveList(gameObject);
    }
    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }
    public override void Encounter()
    {
        if (platformEntity.occupingEntity == null)
        {
            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;
        }
        else if (platformEntity.occupingEntity is MergedButterFly && platformEntity.occupingEntity != this)
        {
            platformEntity.inComingEntity = null;
            var _mergedButterFly = platformEntity.occupingEntity as MergedButterFly;

            foreach (var _butterFly in inputButterFlies)
            {
                _mergedButterFly.inputButterFlies.Add(_butterFly);
            }

            _mergedButterFly.Merge();

            gameObject.SetActive(false);
        }
        else if (platformEntity.occupingEntity is ButterFly && platformEntity.occupingEntity != this)
        {
            var _butterFly = platformEntity.occupingEntity as ButterFly;

            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;

            inputButterFlies.Add(_butterFly);
            Merge();

            _butterFly.gameObject.SetActive(false);
        }
    }
    public void Merge()
    {
        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetFloatArray(inputButterflyColorsShaderID,
                                                  inputButterFlies.Select(bfGraphics => bfGraphics.GetColor())
                                                                  .SelectMany(color => new[] { color.r, color.g, color.b })
                                                                  .ToArray());
        materialPropertyBlock.SetInt(numberOfInputButterfliesShaderID, inputButterFlies.Count);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
    }
}
