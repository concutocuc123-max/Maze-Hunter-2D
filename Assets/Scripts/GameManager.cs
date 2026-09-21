using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int score = 0;
    public int health = 3;
    public float timeRemaining = 60f;
    public int coinsRequiredToPass = 10;
    private int currentCoins = 0;
    
    [Tooltip("MainMenu")]
    public string mainMenuSceneName = "MainMenu"; 

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;

    [Header("Win Panel Details")]
    public TextMeshProUGUI winCoinsText;
    public TextMeshProUGUI winTimeText;

    [Header("Audio Settings")]
    public AudioSource bgmSource;       
    public AudioSource sfxSource;       
    public AudioClip bgmClip;           
    public AudioClip coinSFX;         
    public AudioClip damageSFX;       
    public AudioClip winSFX;           
    public AudioClip loseSFX;          
    public AudioClip buttonClickSFX;   

    [Header("BGM Toggle UI")]
    public TextMeshProUGUI bgmButtonText; 

    private bool isGameOver = false;
    private bool isPaused = false;
    private bool isBGMMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            TogglePause();
        }

        if (isGameOver || isPaused) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            timeRemaining = 0;
            UpdateUI();
            GameOver(false); // Hết giờ -> Thua
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);
            PlaySFX(buttonClickSFX);
        }
    }
    public void ToggleBGM()
    {
        PlaySFX(buttonClickSFX);

        if (bgmSource != null)
        {
            isBGMMuted = !isBGMMuted;
            bgmSource.mute = isBGMMuted; 

            if (bgmButtonText != null)
            {
                bgmButtonText.text = isBGMMuted ? "Music: OFF" : "Music: ON";
            }
        }
    }

    public void ResumeGame()
    {
        PlaySFX(buttonClickSFX);
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void AddCoin()
    {
        score += 10;
        currentCoins++;
        PlaySFX(coinSFX);
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;

        PlaySFX(damageSFX);
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
        Time.timeScale = 0; // Dừng thời gian game

        // Dừng Nhạc nền khi kết thúc
        if (bgmSource != null) bgmSource.Stop();

        if (isWin)
        {
            if (winCoinsText != null) winCoinsText.text = "Coins: " + currentCoins + "/" + coinsRequiredToPass;
            if (winTimeText != null) winTimeText.text = "Time Left: " + Mathf.CeilToInt(timeRemaining) + "s";

            if (winPanel != null) winPanel.SetActive(true);
            PlaySFX(winSFX);
        }
        else
        {
            if (losePanel != null) losePanel.SetActive(true);
            PlaySFX(loseSFX);
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null) 
            scoreText.text = "Coins: " + currentCoins + "/" + coinsRequiredToPass;
        if (healthText != null) healthText.text = "Health: " + health;
        if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString() + "s";
    }

    public void RestartLevel()
    {
        PlaySFX(buttonClickSFX);
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        PlaySFX(buttonClickSFX);
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        PlaySFX(buttonClickSFX);
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        PlaySFX(buttonClickSFX);
        Debug.Log("Đã thoát ứng dụng!");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}