using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BasickScreen
{
    public GameManager Manager;
    public Button profileButton;
    public Button gameButton;
    public Button rulesButton;
    public Button storiesButton;
    public TMP_Text score;
    public BasickScreen profile;
    public BasickScreen rules;
    public BasickScreen game;
    public BasickScreen stories;
    public BasickScreen tutorial;
    

    private TextManager _textManager = new TextManager();

    public override void Subscribe()
    {

        base.Subscribe();

        profileButton.onClick.AddListener(Profile);
        gameButton.onClick.AddListener(GameStart);
        rulesButton.onClick.AddListener(Rules);
        storiesButton.onClick.AddListener(Stories);

        Show();

        if (PlayerPrefs.GetInt("FirstGame") == 0)
        {
            tutorial.Show();
        }

        if (PlayerPrefs.GetInt("NextGame") == 1)
        {
            GameStart();
            PlayerPrefs.SetInt("NextGame", 0);
        }
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        profileButton.onClick.RemoveListener(Profile);
        gameButton.onClick.RemoveListener(GameStart);
        rulesButton.onClick.RemoveListener(Rules);
        storiesButton.onClick.RemoveListener(Stories);
    }

    public override void Show()
    {
        base.Show();
        SetScreen();
    }


    private void SetScreen()
    {
        _textManager.SetText(PlayerPrefs.GetInt("Score"), score, true);
    }

    public void UpdateScore()
    {
        _textManager.SetText(PlayerPrefs.GetInt("Score"), score, true);
    }

    private void Profile()
    {
        profile.Show();
    }

    private void GameStart()
    {
        Hide();
        game.Show();
        //Manager.RestartLevel();
    }

    private void Rules()
    {
        rules.Show();
    }
    private void Stories()
    {
        stories.Show();
    }
}
