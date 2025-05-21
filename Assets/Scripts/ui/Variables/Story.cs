using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Story : BasickScreen
{
    public Image storyImage;
    public TMP_Text title;
    public TMP_Text description;

    public StoryData[] _stories;

    [Serializable]
    public class StoryData
    {
        public Sprite image;
        public string title;
        [SerializeField, TextArea]
        public string description;
    }

    private int _currentStoryIndex;

    public void SetIndex(int index)
    {
        _currentStoryIndex = index;
    }

    public override void Show()
    {
        base.Show();
        StoryData currentStory = _stories[_currentStoryIndex];
        storyImage.sprite = currentStory.image;
        title.text = currentStory.title;
        description.text = currentStory.description;
    }
}
