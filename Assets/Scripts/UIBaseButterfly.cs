using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBaseButterfly : MonoBehaviour
{
    public GameEvent lockInput;
    public GameEvent crossLanded;
    public Image butterflyWings;
    public Image cross;

    public void CrossSequence()
    {
        lockInput.Raise();

        cross.DOFade(1, 0.5f).OnComplete(() =>
        {
            cross.rectTransform.DOAnchorPos(Vector2.zero, 0.35f).OnComplete(CrossLandedRaise);
            cross.rectTransform.DOScale(Vector3.one, 0.35f);
        });
    }

    public void Clear()
    {
        cross.DOKill();
        cross.rectTransform.DOKill();

        cross.color = new Color(1, 0, 0, 0);
        cross.rectTransform.localScale = new Vector3(2, 2, 2);
        cross.rectTransform.anchoredPosition = new Vector2(-200, -200);
        gameObject.SetActive(false);
    }
    void CrossLandedRaise()
    {
        crossLanded.Raise();
    }
}
