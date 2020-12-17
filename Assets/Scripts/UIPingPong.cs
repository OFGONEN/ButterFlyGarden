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
    public Button uiButton;
    public Vector2 destination;
    public float speed;
    public Vector2 position;

    private void Start()
    {
        position = uiRectTransform.anchoredPosition;
    }

    [Button]
    public void GoDestination()
    {
        if (uiButton != null)
            uiButton.interactable = false;

        var _temp = destination;
        destination = position;
        position = _temp;

        uiRectTransform.DOComplete();
        uiRectTransform.DOAnchorPos(_temp, speed).OnComplete(EnableButtonInteraction);
    }

    void EnableButtonInteraction()
    {
        if (uiButton != null)
            uiButton.interactable = true;
    }
}
