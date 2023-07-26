using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas _pauseMenuUI;
    [SerializeField] private Canvas _optionsMenuUI;
    [SerializeField] AudioMixer _mainAudioMixer;
    [SerializeField] TextMeshProUGUI _currentResolutionText;
    [SerializeField] TextMeshProUGUI _currentFullscreenText;
    private bool _isFullscreen;
    private bool _isPaused;
    private bool _isInOptions;
    private bool foundRes;

    public List<ResItem> _resolutions = new List<ResItem>();
    private int _selectedResolution;

    //Full screen toogle and resolution option needs testing once build errors are fixed
    //Fix bug where player can still look around while paused

    private void Awake()
    {
        //defaulting game to fullscreen
        Screen.fullScreen = true;
        _isFullscreen = true;
        _currentFullscreenText.text = "Fullscreen";
        //checking for correct default resolution
        foundRes = false;
        for (int i = 0; i < _resolutions.Count; i++)
        {
            if(Screen.width == _resolutions[i].width && Screen.height == _resolutions[i].height)
            {
                foundRes =  true;
                _selectedResolution = i;
                SetRes();
            }
        }
        //in case user has a resolution not in the list
        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.width = Screen.width;
            newRes.height = Screen.height;
            _resolutions.Add(newRes);
            _selectedResolution = _resolutions.Count - 1;
            SetRes();
        }
    }
    private void Update()
    {
        //escape pauses game and pressing it again un pauses
        if (Keyboard.current.escapeKey.wasPressedThisFrame && _isPaused == false && !_isInOptions)
        {
            PauseGame();
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && _isPaused == true && !_isInOptions)
        {
            UnpauseGame();
        }
        else if (Keyboard.current.backspaceKey.wasPressedThisFrame && _isPaused == true && _isInOptions)
        {
            _optionsMenuUI.gameObject.SetActive(false);
            _pauseMenuUI.gameObject.SetActive(true);
            _isInOptions = false;
            _isPaused = true;
        }
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        _pauseMenuUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        _isPaused = false;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        _pauseMenuUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _isPaused = true;
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
        _isFullscreen = !_isFullscreen;
        if(_isFullscreen)
        {
            _currentFullscreenText.text = "Fullscrene";
        }
        else if (!_isFullscreen)
        {
            _currentFullscreenText.text = "Windowed";
        }
    }

    public void SetVolume(float volume)
    {
        _mainAudioMixer.SetFloat("volume", volume);
    }

    public void ResLeft()
    {
        _selectedResolution--;
        if(_selectedResolution < 0)
        {
            _selectedResolution = 0;
        }
        SetRes();
    }

    public void ResRight()
    {
        _selectedResolution++;
        if(_selectedResolution > _resolutions.Count - 1)
        {
            _selectedResolution = _resolutions.Count - 1;
        }
        SetRes();
    }

    private void SetRes()
    {
        _currentResolutionText.text = _resolutions[_selectedResolution].width.ToString() + "x" + _resolutions[_selectedResolution].height.ToString();
        Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, _isFullscreen);
    }
}

[System.Serializable]
public class ResItem
{
    public int width, height;
}
