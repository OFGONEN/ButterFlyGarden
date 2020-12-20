using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelperAsset : MonoBehaviour
{
    public RectTransform uiRectTransform;
    public Image uiImage;
    public TextMeshProUGUI uiText;

    [HideInInspector]
    public Vector3[] uiWorldCordinates;

    private void Awake()
    {
        uiWorldCordinates = new Vector3[4];
        uiRectTransform.GetWorldCorners(uiWorldCordinates);
    }

    public void ResetAsset()
    {
        if (uiImage != null)
            uiImage.color = Color.white;

        if (uiText != null)
            uiText.color = Color.black;
    }
}
