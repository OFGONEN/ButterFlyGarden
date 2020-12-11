using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : OccupyingEntity
{
    public Color color;

    private void Start()
    {
        renderer.material.color = color;
    }
}
