using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergedButterFly : ButterFly
{
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

        occupyingEntitySet.itemDictionary.Add(mapCord, this);
        occupyingEntitySet.itemList.Add(this);

        butterFlySet.itemDictionary.Add(mapCord, this);
        butterFlySet.itemList.Add(this);
    }
    private void OnDisable()
    {
        occupyingEntitySet.itemList.Remove(this);
        occupyingEntitySet.itemDictionary.Remove(mapCord);

        butterFlySet.itemList.Remove(this);
        butterFlySet.itemDictionary.Remove(mapCord);
    }
    public override void Encounter()
    {
        if (platformEntity.occupingEntity == null)
        {
            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;
        }
        else if (platformEntity.occupingEntity is MergedButterFly)
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
        else if (platformEntity.occupingEntity is ButterFly)
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
