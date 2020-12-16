using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    public Renderer renderer;
    public Transform graphicTransform;
    public MaterialPropertyBlock materialPropertyBlock;
    public Animator entityAnimator;
    public Vector2 mapCord;

    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public bool hasData = false;

    public abstract void SetData();
    public abstract void ResetToDefault();
}
