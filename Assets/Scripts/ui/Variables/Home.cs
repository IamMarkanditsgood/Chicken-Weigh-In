using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Home : BasickScreen
{
    public Button profileButton;
    public TMP_Text score;
    public TMP_Text currentLevel;
    public BasickScreen profile;
    public BasickScreen tutorial;
    public BasickScreen help;
    public BasickScreen restart;

    public Button helpBuster;
    public Button restartBuster;

    private TextManager _textManager = new TextManager();

    
    public override void Subscribe()
    {
        base.Subscribe();
    
        profileButton.onClick.AddListener(Profile);
        helpBuster.onClick.AddListener(HelpBuster);
        restartBuster.onClick.AddListener(RestartBuseter);

        Show();

        if (PlayerPrefs.GetInt("FirstGame") == 0)
        {
            tutorial.Show();
        }
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
    
        profileButton.onClick.RemoveListener(Profile);
        helpBuster.onClick.RemoveListener(HelpBuster);
        restartBuster.onClick.RemoveListener(RestartBuseter);
    }

    public override void Show()
    {
        base.Show();
        SetScreen();
    }


    private void SetScreen()
    {
        _textManager.SetText(PlayerPrefs.GetInt("Score"), score, true);
        currentLevel.text = "Level " +( PlayerPrefs.GetInt("Level") +1);
    }

    public void UpdateScore()
    {
        _textManager.SetText(PlayerPrefs.GetInt("Score"), score, true);
    }

    public void UpdateLevel()
    {
        currentLevel.text = "Level " + (PlayerPrefs.GetInt("Level") + 1);
    }
    private void Profile()
    {
        profile.Show();
    }

    private void HelpBuster()
    {
        if(PlayerPrefs.GetInt("Score") >=2000)
        {  
            help.Show();
        }
    }

    private void RestartBuseter()
    {
        if (PlayerPrefs.GetInt("Score") >= 500)
        {
            restart.Show();
        }
    }

}
