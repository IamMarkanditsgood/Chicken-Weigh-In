using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : BasickScreen
{
    public Button next1Button;
    public Button next2Button;

    public GameObject[] Panels;

    private int currentPanel;

    public override void Show()
    {
        base.Show();
        Panels[0].SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        PlayerPrefs.SetInt("FirstGame", 1);
    }

    public override void Subscribe()
    {
        base.Subscribe();

        next1Button.onClick.AddListener(Next);
        next2Button.onClick.AddListener(Next);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        next1Button.onClick.RemoveListener(Next);
        next2Button.onClick.RemoveListener(Next);
    }

    private void Next()
    {
        currentPanel++;
        foreach (var panel in Panels)
        {
            panel.SetActive(false);
        }

        Panels[currentPanel].SetActive(true);
    }
}
