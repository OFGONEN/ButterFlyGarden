using UnityEngine;
using DG.Tweening;

public class BubbleRotation : MonoBehaviour
{
    public Vector3 rotateBy = new Vector3( 1, 1, 1 );

    private void Start()
    {
        transform.DORotate( rotateBy, 1 ).SetRelative().SetLoops( -1, LoopType.Incremental ).SetEase( Ease.Linear );
    }
}
