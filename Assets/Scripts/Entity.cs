using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    public Renderer renderer;
    public MaterialPropertyBlock materialPropertyBlock;
    public Animator entityAnimator;

    [HideInInspector]
    public Vector2 mapCord;

    [HideInInspector]
    public Vector2 direction;
}
