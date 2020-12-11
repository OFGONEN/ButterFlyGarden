using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterFly : OccupyingEntity
{
    public Color color;

    private static int ColorShaderID = Shader.PropertyToID("_Color");
    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetColor(ColorShaderID, color);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
    }
    public Color GetColor()
    {
        return color;
    }
}
