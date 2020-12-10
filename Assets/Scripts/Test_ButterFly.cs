using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ButterFly : MonoBehaviour
{
    public SkinnedMeshRenderer renderer;

    public void ChangeWingColor(Color color)
    {
        renderer.material.color = color;
    }
}
