using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class UIPingPong : MonoBehaviour
{
    public RectTransform uiRectTransform;
    public Image targetImage;
    public TextMeshProUGUI uiText;
    public Vector2 destination;
    public float speed;
    Vector2 position;

    private void Start()
    {
        position = uiRectTransform.anchoredPosition;
    }

    [Button]
    public void GoDestination()
    {
        uiRectTransform.DOAnchorPos(destination, speed);

        var _temp = position;
        position = destination;
        destination = _temp;
    }
}
