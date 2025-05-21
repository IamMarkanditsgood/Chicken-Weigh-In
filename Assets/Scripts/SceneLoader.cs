using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private int sceneIndexToLoad;

    private void Start()
    {
        PlayerPrefs.SetInt("NextGame", 0);
        StartCoroutine(LoadSceneAsync(sceneIndexToLoad));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (image != null)
                image.fillAmount = progress;

            if (loadingText != null)
                loadingText.text = $"{Mathf.RoundToInt(progress * 100)}%";

            yield return null;
        }
    }
}
