using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stories : BasickScreen
{
    public MainMenu mainMenu;
    public Story story;
    public Button[] unlockButtons;
    public Button[] readButtons;

    public override void Subscribe()
    {
        base.Subscribe();

        for(int i = 0; i < unlockButtons.Length; i++)
        {
            int index = i;
            unlockButtons[index].onClick.AddListener(() => UnlockStory(index));
        }
        for (int i = 0; i < readButtons.Length; i++)
        {
            int index = i;
            readButtons[index].onClick.AddListener(() => ReadStory(index));
        }
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        for (int i = 0; i < unlockButtons.Length; i++)
        {
            int index = i;
            unlockButtons[index].onClick.RemoveListener(() => UnlockStory(index));
        }
        for (int i = 0; i < readButtons.Length; i++)
        {
            int index = i;
            readButtons[index].onClick.RemoveListener(() => ReadStory(index));
        }
    }

    public override void Show()
    {
        base.Show();
        SetScreen();

        
    }

    private void SetScreen()
    {
        for (int i = 0; i < unlockButtons.Length; i++)
        {
            string key = "Story" + i;
            if (PlayerPrefs.GetInt(key) == 1)
            {
                unlockButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void UnlockStory(int index)
    {
        int score = PlayerPrefs.GetInt("Score");
        if (score >= 2000)
        {
            score -= 2000;
            PlayerPrefs.SetInt("Score", score);

            string key = "Story" + index;
            PlayerPrefs.SetInt(key, 1);

            mainMenu.UpdateScore();

            SetScreen();
        }
    }
    private void ReadStory(int index)
    {
        story.SetIndex(index);
        story.Show();
    }
}
