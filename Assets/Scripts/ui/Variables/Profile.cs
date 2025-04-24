using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : BasickScreen
{
    public AvatarManager avatarManager;
    public TMP_Text _name;
    public TMP_InputField _nameInput;

    public Image firstAchieve;
    public Sprite openAchieve;

    public Button editName;
    public Button editAvatar;
    public Button rulesButton;
    public BasickScreen RulesScreen;

    public override void Subscribe()
    {
        base.Subscribe();

        editName.onClick.AddListener(EditName);
        editAvatar.onClick.AddListener(EditAvatar);
        rulesButton.onClick.AddListener(Rules);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        editName.onClick.RemoveListener(EditName);
        editAvatar.onClick.RemoveListener(EditAvatar);
        rulesButton.onClick.RemoveListener(Rules);
    }

    public override void Show()
    {
        base.Show();
        SetScreen();
    }

    public override void Hide()
    {
        base.Hide();
        PlayerPrefs.SetString("Name", _nameInput.text);
    }

    private void SetScreen()
    {
        avatarManager.SetSavedPicture();
        _name.text = PlayerPrefs.GetString("Name", "UserName");
        _nameInput.text = PlayerPrefs.GetString("Name", "UserName");


        if(PlayerPrefs.GetInt("Achieve") == 1)
        {
            firstAchieve.sprite = openAchieve;
        }
    }


    private void EditName()
    {
        if (_nameInput.gameObject.activeInHierarchy)
        {
            _nameInput.gameObject.SetActive(false);
            _name.gameObject.SetActive(true);
            _name.text = _nameInput.text;
            PlayerPrefs.SetString("Name", _nameInput.text);
        }
        else
        {
            _nameInput.gameObject.SetActive(true);
            _name.gameObject.SetActive(false);
        }
    }
    private void EditAvatar()
    {
        avatarManager.PickFromGallery();
    }
    private void Rules()
    {
        RulesScreen.Show();
    }
}
