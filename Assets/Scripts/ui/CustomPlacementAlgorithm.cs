using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomPlacementAlgorithm : MonoBehaviour
{
    public RectTransform panel; // ������ � ������
    public RectTransform[] positions; // ����� ��������� �������
    private int[] positionsUsed; // ³������, �� ������� ��� ������

    void Start()
    {
        // ���������� ����� ��� ���������� �������� �������
        positionsUsed = new int[positions.Length]; 
    }

    public void SortEggsInPanel()
    {
        if (panel == null)
        {
            Debug.LogError("Panel �� ����������!");
            return;
        }

        if (positions == null || positions.Length == 0)
        {
            Debug.LogError("������� �� ���������!");
            return;
        }

        // �������� �� ����
        List<RectTransform> eggs = new List<RectTransform>();
        foreach (Transform child in panel)
        {
            if (child.TryGetComponent<RectTransform>(out var egg))
            {
                eggs.Add(egg);
            }
        }

        if (eggs.Count == 0) return;

        // ������� ���������� ������� (��� ���, �� ��� ������)
        for (int i = 0; i < positionsUsed.Length; i++)
        {
            if (positionsUsed[i] == 0) // 0 = ����� �������
            {
                // ����������, �� �� ��� ������� ��� � ����
                bool positionOccupied = false;
                foreach (RectTransform egg in eggs)
                {
                    if (Vector2.Distance(egg.anchoredPosition, positions[i].anchoredPosition) < 1f)
                    {
                        positionOccupied = true;
                        positionsUsed[i] = 1; // ��������� �� �������
                        break;
                    }
                }

                if (!positionOccupied)
                {
                    positionsUsed[i] = 0;
                }
            }
        }

        // �������� ���� �� ������ ��������
        int eggIndex = 0;
        for (int posIndex = 0; posIndex < positions.Length && eggIndex < eggs.Count; posIndex++)
        {
            if (positionsUsed[posIndex] == 0) // ���� ������� �����
            {
                eggs[eggIndex].anchoredPosition = positions[posIndex].anchoredPosition;
                positionsUsed[posIndex] = 1; // ��������� �� �������
                eggIndex++;
            }
        }

        // ���� ���������� ���� (�� ��������� �������)
        for (; eggIndex < eggs.Count; eggIndex++)
        {
            // ��������� ����� ����� ������� (���� ���� ��� ������� ����� �����)
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
