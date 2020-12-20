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
    Vector2 position;
    public float speed;
    public bool safeArea;


    private void Start()
    {
        position = uiRectTransform.anchoredPosition;

        if (safeArea)
            destination += new Vector2(0, Mathf.Sign(destination.y) * (Screen.height - Screen.safeArea.height - Screen.safeArea.position.y));
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
