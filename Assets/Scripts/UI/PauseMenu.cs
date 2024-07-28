using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused = false;

    public GameObject pauseMenu;
    
    public AudioMixer masterMixer;

    public UnityEvent onQuitToMenu;

    private void Start()
    {
        UnpauseGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) UnpauseGame();
            else PauseGame();
        }
    }

    public void QuitToMenu()
    {
        if (!_isPaused) return;
        
        // TODO: Load main menu.
        onQuitToMenu?.Invoke();
    }

    public void Resume()
    {
        if (!_isPaused) return;
        
        UnpauseGame();
    }

    public void SetMasterVolume(Slider volume)
    {
        masterMixer?.SetFloat("master", volume.value);
    }

    public void SetBGMVolume(Slider volume)
    {
        masterMixer?.SetFloat("bgm", volume.value);
    }

    public void SetSFXVolume(Slider volume)
    {
        masterMixer?.SetFloat("sfx", volume.value);
    }

    private void PauseGame()
    {
        if (pauseMenu is null) return;
        pauseMenu.SetActive(true);
        
        Time.timeScale = 0;
        _isPaused = true;
    }

    private void UnpauseGame()
    {
        if (pauseMenu is null) return;
        pauseMenu.SetActive(false);
        
        Time.timeScale = 1;
        _isPaused = false;
    }
}