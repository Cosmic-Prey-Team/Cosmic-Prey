using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using TMPro;

public enum GameState
{
    GameIsPaused,
    GameIsUnpaused,
    InOptionsMenu,
    PlayerIsDead,
    PlayerIsAlive,
    WhaleIsAlive,
    WhaleIsDefeated,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameState _gameState;
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

    private void Awake()
    {
        Time.timeScale = 1;
        if(Instance == null)
        {
            transform.SetParent(null);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
        _gameState = GameState.GameIsUnpaused;

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
        if (Keyboard.current.escapeKey.wasPressedThisFrame && _gameState == GameState.GameIsUnpaused)
        {
            PauseGame();
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && _gameState == GameState.GameIsPaused)
        {
            UnpauseGame();
        }
        else if (Keyboard.current.backspaceKey.wasPressedThisFrame && _gameState == GameState.InOptionsMenu)
        {
            _optionsMenuUI.gameObject.SetActive(false);
            if (_pauseMenuUI != null)
            {
                _pauseMenuUI.gameObject.SetActive(true);
            }
            _isInOptions = false;
            _isPaused = true;
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        if(_pauseMenuUI != null)
        {
            _pauseMenuUI.gameObject.SetActive(false);
        }
        InputHandler.ModifyCursorState(true, true);
        _isPaused = false;
        _gameState = GameState.GameIsUnpaused;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        if (_pauseMenuUI != null)
        {
            _pauseMenuUI.gameObject.SetActive(true);
        }
        InputHandler.ModifyCursorState(false, false);
        _isPaused = true;
        _gameState = GameState.GameIsPaused;
    }

    public void OptionsButton()
    {
        _gameState = GameState.InOptionsMenu;
    }

    public void ExitGameButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToogleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        _isFullscreen = !_isFullscreen;
        if(_isFullscreen)
        {
            _currentFullscreenText.text = "Fullscreen";
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

    public void OnHover(Image hoveredImage)
    {
        this.transform.parent.gameObject.SetActive(false);
        hoveredImage.gameObject.SetActive(true);
    }

    public void OnExitHover(Image unhoveredImage)
    {
        this.transform.parent.gameObject.SetActive(false);
        unhoveredImage.gameObject.SetActive(true);
    }
}

[System.Serializable]
public class ResItem
{
    public int width, height;
}
