using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite highlightedSprite;

    private Sprite noHighlightedSprite;
    private Image buttonImage;

    void Start()
    {
        buttonImage = gameObject.GetComponent<Image>();
        noHighlightedSprite = buttonImage.overrideSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.overrideSprite = highlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.overrideSprite = noHighlightedSprite;
    }
}
