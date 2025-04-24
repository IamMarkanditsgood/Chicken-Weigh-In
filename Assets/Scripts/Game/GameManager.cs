using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CustomPlacementAlgorithm customPlacementAlgorithm;
    public Image[] arrows;
    public Image ceneter;

    public Sprite[] arrowsSprite;
    public Sprite ceneterSprite;

    public Sprite[] arrowsSpriteNo;
    public Sprite ceneterSpriteNo;

    public Home homeScreen;
    public BasickScreen win;
    public BasickScreen lose;
    public BasickScreen help;
    public BasickScreen restart;

    public TMP_Text rewardText;
    public static GameManager Instance;

    public GameObject[] eggPrefabs;
    public Transform bottomPanel;
    public DropZone leftPlatform;
    public DropZone rightPlatform;

    public RectTransform left;
    public RectTransform right;



    public Button restartButton;
    public Button autoPlaceButton;

    public Transform eggPanel; // контейнер з якого беруть яйця

    public float platformMoveAmount = 50f;
    public float platformMoveSpeed = 5f;

    private List<int[]> levelEggSets = new List<int[]>
    {
        new int[]{1, 4, 3, 2, 5, 2, 2, 1},
        new int[]{3, 1, 4, 2, 2, 4, 4},
        new int[]{5, 1, 2, 3, 3, 1, 4, 1},
        new int[]{2, 3, 5, 4, 2, 1, 1, 2},
        new int[]{3, 3, 3, 2, 2, 2, 1, 1, 3},
        new int[]{1, 2, 3, 4, 5, 1, 3, 1},
        new int[]{5, 4, 3, 2, 1, 2, 1, 2},
        new int[]{1, 1, 2, 2, 3, 3, 4, 4},
        new int[]{2, 2, 2, 3, 3, 4, 4},
        new int[]{5, 5, 1, 1, 2, 3, 3}
    };

    private int[] currentEggSet; // Для збереження поточного набору яєць

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnEggs();


        restartButton.onClick.AddListener(RestartLevel);
        autoPlaceButton.onClick.AddListener(AutoPlaceEggsForWin);
    }

    void SpawnEggs()
    {
        // Вибір випадкового набору яєць
        currentEggSet = levelEggSets[Random.Range(0, levelEggSets.Count)];

        foreach (int weight in currentEggSet)
        {
            GameObject prefab = eggPrefabs[weight - 1];
            GameObject eggObj = Instantiate(prefab, bottomPanel);
            Egg egg = eggObj.GetComponent<Egg>();
            egg.weight = weight;
        }
    }

    public void RestartLevel()
    {
        restart.Hide();
        int newAmoun = PlayerPrefs.GetInt("Score");
        newAmoun -= 500;
        PlayerPrefs.SetInt("Score", newAmoun);
        homeScreen.UpdateScore();
        // Очищення платформ від яєць
        ClearPlatform(leftPlatform);
        ClearPlatform(rightPlatform);

        // Очищення панелі яєць від вже розміщених яєць
        foreach (Transform child in eggPanel)
        {
            Destroy(child.gameObject);
        }

        // Повертаємо яйця в контейнер
        foreach (Transform child in leftPlatform.transform)
        {
            child.SetParent(eggPanel);
        }

        foreach (Transform child in rightPlatform.transform)
        {
            child.SetParent(eggPanel);
        }

        // Відновлюємо яйця з того самого набору, що був на початку
        foreach (int weight in currentEggSet)
        {
            GameObject prefab = eggPrefabs[weight - 1];
            GameObject eggObj = Instantiate(prefab, eggPanel);
            Egg egg = eggObj.GetComponent<Egg>();
            egg.weight = weight;
        }

        Invoke("EvaluateBalance", 0.1f);
    }

    void ClearPlatform(DropZone platform)
    {
        // Очищення платформи від яєць
        foreach (Transform child in platform.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AutoPlaceEggsForWin()
    {
        help.Hide();
        int newAmoun = PlayerPrefs.GetInt("Score");
        newAmoun -= 2000;
        PlayerPrefs.SetInt("Score", newAmoun);
        homeScreen.UpdateScore();
        // Очищаємо платформи
        ClearPlatform(leftPlatform);
        ClearPlatform(rightPlatform);

        // Видаляємо яйця з початкової панелі
        foreach (Transform child in eggPanel)
        {
            Destroy(child.gameObject);
        }

        // Створюємо копію поточного набору яєць
        int[] eggs = currentEggSet;

        List<int> sortedEggs = new List<int>(eggs);
        sortedEggs.Sort((a, b) => b.CompareTo(a)); // спочатку важчі яйця

        List<int> leftEggs = new List<int>();
        List<int> rightEggs = new List<int>();

        int leftWeight = 0;
        int rightWeight = 0;

        foreach (int weight in sortedEggs)
        {
            if (leftWeight <= rightWeight)
            {
                leftEggs.Add(weight);
                leftWeight += weight;
            }
            else
            {
                rightEggs.Add(weight);
                rightWeight += weight;
            }
        }

        MoveEggsToPlatform(leftEggs, leftPlatform);
        MoveEggsToPlatform(rightEggs, rightPlatform);
        arrows[0].sprite = arrowsSprite[0];
        arrows[1].sprite = arrowsSprite[1];
        ceneter.sprite = ceneterSprite;
        StartCoroutine(CheckWinConditionWithDelay());
    }

    void MoveEggsToPlatform(List<int> eggs, DropZone platform)
    {
        foreach (int weight in eggs)
        {
            GameObject prefab = eggPrefabs[weight - 1];
            GameObject eggObj = Instantiate(prefab, platform.transform);
            Egg egg = eggObj.GetComponent<Egg>();
            egg.weight = weight;
        }
    }

    public void EvaluateBalance()
    {
        arrows[0].sprite = arrowsSpriteNo[0];
        arrows[1].sprite = arrowsSpriteNo[1];
        ceneter.sprite = ceneterSpriteNo;
        int leftWeight = CalculateWeight(leftPlatform);
        int rightWeight = CalculateWeight(rightPlatform);

        float leftTargetY = 0;
        float rightTargetY = 0;

        if (leftWeight > rightWeight)
        {
            leftTargetY = -platformMoveAmount;
            rightTargetY = platformMoveAmount;
        }
        else if (leftWeight < rightWeight)
        {
            leftTargetY = platformMoveAmount;
            rightTargetY = -platformMoveAmount;
        }
        else
        {
            arrows[0].sprite = arrowsSprite[0];
            arrows[1].sprite = arrowsSprite[1];
            ceneter.sprite = ceneterSprite;
        }

        StopAllCoroutines();
        StartCoroutine(MovePlatformTo(left, leftTargetY));
        StartCoroutine(MovePlatformTo(right, rightTargetY));
    }

    int CalculateWeight(DropZone zone)
    {
        int total = 0;
        foreach (Transform child in zone.transform)
        {
            Egg egg = child.GetComponent<Egg>();
            if (egg != null)
                total += egg.weight;
        }
        return total;
    }

    IEnumerator MovePlatformTo(RectTransform platform, float targetY)
    {
        Vector2 startPos = platform.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, targetY);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * platformMoveSpeed;
            platform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
    }


    public IEnumerator CheckWinConditionWithDelay()
    {
        yield return null;
        Debug.Log(eggPanel.childCount);
        if (eggPanel.childCount <= 0)
        {
            Debug.Log(CalculateWeight(leftPlatform) + CalculateWeight(rightPlatform));
            int leftWeight = CalculateWeight(leftPlatform);
            int rightWeight = CalculateWeight(rightPlatform);

            if (leftWeight == rightWeight)
            {
                Debug.Log("🎉 Перемога!");
                int newAmoun = PlayerPrefs.GetInt("Score");
                newAmoun += 100 * (PlayerPrefs.GetInt("Level") + 1);
                PlayerPrefs.SetInt("Score", newAmoun);
                homeScreen.UpdateScore();

                int newLevel = PlayerPrefs.GetInt("Level");
                newLevel++;
                PlayerPrefs.SetInt("Level", newLevel);

                rewardText.text = "+" + 100 * (PlayerPrefs.GetInt("Level"));
                yield return new WaitForSeconds(1);
                win.Show();
                PlayerPrefs.SetInt("Achieve", 1);
            }
            else
            {
                Debug.Log("❌ Програш!");
                yield return new WaitForSeconds(1);
                lose.Show();
            }
        }
    }
}
