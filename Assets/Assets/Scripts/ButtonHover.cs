using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public TitleScreenController controller;

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.MoveArrow(transform as RectTransform);
    }
}