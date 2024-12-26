using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    Intro,

    Playing,

    Dead
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState state = GameState.Intro;

    public float PlayStartTime;

    public int Lives = 3;

    [Header("Reference")]
    public GameObject IntroUI;

    public GameObject DeadUI;

    public GameObject EnemySpawner;

    public GameObject FoodSpawner;

    public GameObject GoldenSpawner;

    public Player PlayerScript;

    public TMP_Text ScoreText;

    public TMP_Text GameSpeedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        IntroUI.SetActive(true);
    }

    float CalculateScore()
    {
        return Time.time - PlayStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore());
        int currentHighScore = PlayerPrefs.GetInt("highScore");
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            PlayerPrefs.Save();
        }
    }

    int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }

    public float CalculateGameSpeed()
    {
        if(state != GameState.Playing)
        {
            return 5f;
        }
        float speed = 5f + Mathf.Log10(CalculateScore() + 1) * 2f;
        speed = Mathf.Clamp(speed, 5f, 20f); // Mathf.Min을 Clamp로 수정
        //Debug.Log($"Calculated Speed: {speed}");
        return speed;
    }
    
    void Update()
    {
        if(state == GameState.Playing)
        {
            ScoreText.text = "Score : " + Mathf.FloorToInt(CalculateScore());

            float speed = CalculateGameSpeed();
            GameSpeedText.text = "Game Speed : " + Mathf.FloorToInt(speed);
            Debug.Log($"Updated Game Speed: {speed}");
        }
        else if(state == GameState.Dead)
        {
            ScoreText.text = "High Score : " + GetHighScore();
        }
        if(state== GameState.Intro && Input.GetKeyDown(KeyCode.Space))
        {
            state= GameState.Playing;
            IntroUI.SetActive(false);

            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldenSpawner.SetActive(true);
            PlayStartTime = Time.time;
        }
        if(state == GameState.Playing && Lives == 0)
        {
            PlayerScript.KillPlayer();

            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpawner.SetActive(false);
            DeadUI.SetActive(true);

            SaveHighScore();

            state = GameState.Dead;
        }
        if(state == GameState.Dead && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("main");
        }
    }
}
