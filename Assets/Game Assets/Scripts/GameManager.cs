using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas _pauseMenuUI;
    [SerializeField] private Canvas _optionsMenuUI;
    [SerializeField] AudioMixer _mainAudioMixer;
    private bool _isPaused;
    private bool _isInOptions;

    //Full screen toogle needs testing once build errors are fixed
    //Need to set _isInOptions back to false whenever leaving the options menu
    //Need to set up Resolution method
    //Fix bug where player can still look around while paused

    private void Awake()
    {
        //defaulting game to fullscreen
        Screen.fullScreen = true;
    }
    private void Update()
    {
        //escape pauses game and pressing it again un pauses
        if (Keyboard.current.escapeKey.wasPressedThisFrame && _isPaused == false && !_isInOptions)
        {
            Time.timeScale = 0;
            _pauseMenuUI.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _isPaused = true;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && _isPaused == true && !_isInOptions)
        {
            Time.timeScale = 1;
            _pauseMenuUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            _isPaused = false;
        }
    }

    public void ContinueButton()
    {
        _pauseMenuUI.gameObject.SetActive(false);
        _isPaused = false;
        Time.timeScale = 1;
        _pauseMenuUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void OptionsButton()
    {
        _pauseMenuUI.gameObject.SetActive(false);
        _optionsMenuUI.gameObject.SetActive(true);
        _isInOptions = true;
    }

    public void ExitGameButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ToogleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetVolume(float volume)
    {
        _mainAudioMixer.SetFloat("volume", volume);
    }

    public void SetResolution()
    {
        //set resolution
    }
}
