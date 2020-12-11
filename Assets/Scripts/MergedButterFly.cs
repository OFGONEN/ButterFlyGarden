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


    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetTexture("_MainTex", patternTexture);

        // Array sizes are determined the first time they are set, so set it to max beforehand.
        materialPropertyBlock.SetFloatArray(inputButterflyColorsShaderID, new float[3 * 10]);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
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
