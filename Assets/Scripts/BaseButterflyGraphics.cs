using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseButterflyGraphics : MonoBehaviour
{
    private Renderer wingsRenderer;
    //private Renderer bodyRenderer;
    private MaterialPropertyBlock wingsMaterialPropertyBlock;

    private static int ColorShaderID = Shader.PropertyToID( "_Color");

    void Start()
    {
        var renderers = GetComponentsInChildren< Renderer >();
        wingsRenderer = renderers[ 0 ];
        //bodyRenderer  = renderers[ 1 ];
        wingsMaterialPropertyBlock = new MaterialPropertyBlock();

        wingsRenderer.GetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
        wingsMaterialPropertyBlock.SetColor( ColorShaderID, Random.ColorHSV() );
        wingsRenderer.SetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
    }

    public Color GetColor()
    {
        wingsRenderer.GetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
        return wingsMaterialPropertyBlock.GetColor( ColorShaderID );
    }
}
