using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public UnityEvent onHoverEnter;
    public UnityEvent onHover;
    public UnityEvent onHoverExit;
    [HideInInspector]
    public PointerEventData lastPointerEventData;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        lastPointerEventData = pointerEventData;
        onHoverEnter?.Invoke();
    }

    public void OnPointerMove(PointerEventData pointerEventData)
    {
        lastPointerEventData = pointerEventData;
        onHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        lastPointerEventData = pointerEventData;
        onHoverExit?.Invoke();
    }
}