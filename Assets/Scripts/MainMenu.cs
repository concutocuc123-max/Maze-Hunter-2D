using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionsPanel; 

    [Header("Audio Settings")]
    public AudioSource bgmSource;         
    public AudioSource sfxSource;         
    public AudioClip buttonClickSFX;    
    public TextMeshProUGUI bgmButtonText; 

    private bool isBGMMuted = false;

    void Start()
    {
        // Đảm bảo thời gian chạy bình thường khi quay lại Menu từ In-game
        Time.timeScale = 1f;

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (instructionsPanel != null && instructionsPanel.activeSelf)
            {
                CloseInstructions();
            }
        }
    }

    public void PlayGame()
    {
        PlaySFX();
        SceneManager.LoadScene("Level1");
    }

    // --- NÚT HƯỚNG DẪN CHƠI ---
    public void OpenInstructions()
    {
        PlaySFX();
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
    }

    public void CloseInstructions()
    {
        PlaySFX();
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }

    public void ToggleBGM()
    {
        PlaySFX();
        if (bgmSource != null)
        {
            isBGMMuted = !isBGMMuted;
            bgmSource.mute = isBGMMuted;

            // Đổi chữ hiển thị trên nút bấm
            if (bgmButtonText != null)
            {
                bgmButtonText.text = isBGMMuted ? "Music: OFF" : "Music: ON";
            }
        }
    }

    public void QuitGame()
    {
        PlaySFX();
        Debug.Log("Đã thoát game!");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void PlaySFX()
    {
        if (sfxSource != null && buttonClickSFX != null)
        {
            sfxSource.PlayOneShot(buttonClickSFX);
        }
    }
}