using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public CustomPlacementAlgorithm customPlacementAlgorithm;
    public void OnDrop(PointerEventData eventData)
    {
        Egg egg = eventData.pointerDrag.GetComponent<Egg>();
        if (egg != null)
        {
            egg.transform.SetParent(transform);
            egg.GetComponent<CanvasGroup>().blocksRaycasts = false;
            egg.GetComponent<Egg>().enabled = false;

            // Перевірка виграшу
            GameManager.Instance.EvaluateBalance();
            StartCoroutine(GameManager.Instance.CheckWinConditionWithDelay());
            //customPlacementAlgorithm.SortEggsInPanel();
        }
    }

    public int GetTotalWeight()
    {
        int sum = 0;
        foreach (Transform child in transform)
        {
            Egg egg = child.GetComponent<Egg>();
            if (egg != null)
                sum += egg.weight;
        }
        return sum;
    }
}
