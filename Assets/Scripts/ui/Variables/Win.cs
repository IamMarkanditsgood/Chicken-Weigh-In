using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : BasickScreen
{
    public Button nextLevel;

    public override void Subscribe()
    {
        base.Subscribe();
        nextLevel.onClick.AddListener(RestartScene);
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        nextLevel.onClick.RemoveListener(RestartScene);
    }

    private void RestartScene()
    {
        PlayerPrefs.SetInt("NextGame", 1);
        SceneManager.LoadScene("MainGame");
    }
}
