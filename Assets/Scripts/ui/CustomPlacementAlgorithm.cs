using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomPlacementAlgorithm : MonoBehaviour
{
    public RectTransform panel; // Панель з яйцями
    public RectTransform[] positions; // Масив доступних позицій
    private int[] positionsUsed; // Відстежує, які позиції вже зайняті

    void Start()
    {
        // Ініціалізуємо масив для відстеження зайнятих позицій
        positionsUsed = new int[positions.Length]; 
    }

    public void SortEggsInPanel()
    {
        if (panel == null)
        {
            Debug.LogError("Panel не призначена!");
            return;
        }

        if (positions == null || positions.Length == 0)
        {
            Debug.LogError("Позиції не призначені!");
            return;
        }

        // Отримуємо всі яйця
        List<RectTransform> eggs = new List<RectTransform>();
        foreach (Transform child in panel)
        {
            if (child.TryGetComponent<RectTransform>(out var egg))
            {
                eggs.Add(egg);
            }
        }

        if (eggs.Count == 0) return;

        // Скидаємо відстеження позицій (крім тих, що вже зайняті)
        for (int i = 0; i < positionsUsed.Length; i++)
        {
            if (positionsUsed[i] == 0) // 0 = вільна позиція
            {
                // Перевіряємо, чи на цій позиції вже є яйце
                bool positionOccupied = false;
                foreach (RectTransform egg in eggs)
                {
                    if (Vector2.Distance(egg.anchoredPosition, positions[i].anchoredPosition) < 1f)
                    {
                        positionOccupied = true;
                        positionsUsed[i] = 1; // Позначаємо як зайняту
                        break;
                    }
                }

                if (!positionOccupied)
                {
                    positionsUsed[i] = 0;
                }
            }
        }

        // Розміщуємо яйця на вільних позиціях
        int eggIndex = 0;
        for (int posIndex = 0; posIndex < positions.Length && eggIndex < eggs.Count; posIndex++)
        {
            if (positionsUsed[posIndex] == 0) // Якщо позиція вільна
            {
                eggs[eggIndex].anchoredPosition = positions[posIndex].anchoredPosition;
                positionsUsed[posIndex] = 1; // Позначаємо як зайняту
                eggIndex++;
            }
        }

        // Якщо залишилися яйця (не вистачило позицій)
        for (; eggIndex < eggs.Count; eggIndex++)
        {
            // Знаходимо першу вільну позицію (може бути вже зайнята іншим яйцем)
            for (int posIndex = 0; posIndex < positions.Length; posIndex++)
            {
                if (positionsUsed[posIndex] == 0)
                {
                    eggs[eggIndex].anchoredPosition = positions[posIndex].anchoredPosition;
                    positionsUsed[posIndex] = 1;
                    break;
                }
            }
        }
    }

}
