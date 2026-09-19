using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int health = 3;
    public float timeRemaining = 60f;
    public int coinsRequiredToPass = 10;
    private int currentCoins = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject losePanel;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            GameOver(false); // Hết giờ -> Thua
        }
    }

    public void AddCoin()
    {
        score += 10;
        currentCoins++;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateUI();
        if (health <= 0)
        {
            GameOver(false); // Hết máu -> Thua
        }
    }

    public bool CanExit()
    {
        return currentCoins >= coinsRequiredToPass;
    }

    public void GameOver(bool isWin)
    {
        isGameOver = true;
        Time.timeScale = 0; // Dừng game
        if (isWin) winPanel.SetActive(true);
        else losePanel.SetActive(true);
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + health;
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString() + "s";
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}