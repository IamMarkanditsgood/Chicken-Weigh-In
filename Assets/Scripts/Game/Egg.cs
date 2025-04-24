using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Egg : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int weight; // 1 - 5
    private Transform parentAfterDrag;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // якщо ми потрапили в DropZone, то батьк≥вство вже буде зм≥нено
        if (transform.parent == canvas.transform)
        {
            // якщо не скинули на зону Ч повертаЇмо назад
            transform.SetParent(parentAfterDrag);
        }

        GameManager.Instance.EvaluateBalance();
    }
}
