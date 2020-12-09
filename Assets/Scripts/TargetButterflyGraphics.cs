using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetButterflyGraphics : MonoBehaviour
{
    public Texture2D patternImage;

    public List< BaseButterflyGraphics > inputButterflies;

    private MaterialPropertyBlock wingsMaterialPropertyBlock;
    private Renderer wingsRenderer;
    //private Renderer bodyRenderer;

    private static readonly int inputButterflyColorsShaderID = Shader.PropertyToID( "inputButterflyColors" );
    private static readonly int numberOfInputButterfliesShaderID = Shader.PropertyToID( "numberOfInputButterflies" );

    void Awake()
    {
        var renderers = GetComponentsInChildren< Renderer >();
        wingsRenderer = renderers[ 0 ];
        //bodyRenderer  = renderers[ 1 ];
        wingsMaterialPropertyBlock = new MaterialPropertyBlock();

        if( patternImage != null )
        {
            wingsRenderer.GetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
            wingsMaterialPropertyBlock.SetTexture( "_MainTex", patternImage );
            // Array sizes are determined the first time they are set, so set it to max beforehand.
            wingsMaterialPropertyBlock.SetFloatArray( inputButterflyColorsShaderID, new float[ 3 * 10 ] );
            wingsRenderer.SetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
        }
    }

    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
            Merge();
    }

    public void Merge()
    {
        wingsRenderer.GetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
        wingsMaterialPropertyBlock.SetFloatArray( inputButterflyColorsShaderID, 
                                                  inputButterflies.Select( bfGraphics => bfGraphics.GetColor() )
                                                                  .SelectMany( color => new[] { color.r, color.g, color.b } )
                                                                  .ToArray() );
        wingsMaterialPropertyBlock.SetInt( numberOfInputButterfliesShaderID, inputButterflies.Count );
        wingsRenderer.SetPropertyBlock( wingsMaterialPropertyBlock, 0 ); // Don't care about the 2nd material of wings.
    }
}
