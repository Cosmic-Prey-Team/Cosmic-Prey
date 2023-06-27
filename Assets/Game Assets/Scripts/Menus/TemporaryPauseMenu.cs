using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPauseMenu : MonoBehaviour
{
    InputHandler _input;

    [SerializeField] GameObject[] _activeAlways;
    [SerializeField] GameObject[] _activeOnPause;

    [SerializeField] bool isPaused = false;

    bool _isButtonDown;

    private void Awake()
    {
        _input = FindObjectOfType<InputHandler>();
    }
    private void Start()
    {
        TogglePause(false);
    }
    private void Update()
    {
        #region Button Press Events
        if (_isButtonDown == false)
        {
            if (_input.pause == true)
            {
                isPaused = !isPaused;

                TogglePause(isPaused); // only on button downs
                _isButtonDown = true;
                //Debug.Log("PressPauseKeyDown()");
            }
        }

        if (_isButtonDown == true)
        {
            if (_input.pause == false)
            {
                _isButtonDown = false;
                //Debug.Log("PressIPauseKeyUp()");
            }
        }
        #endregion
    }

    
    public void TogglePause(bool active)
    {
        Debug.Log("TogglePause()");
        if (active)
        {
            //disble mouse look
            InputHandler.ModifyCursorState(false, false);

            //turn on pause objects
            if(_activeOnPause.Length > 0)
            {
                foreach (var uiObject in _activeOnPause)
                {
                    if (uiObject.activeInHierarchy == false) uiObject.SetActive(true);
                }
            }
            
            //turn off permanent objects
            if(_activeAlways.Length > 0)
            {
                foreach (var uiObject in _activeAlways)
                {
                    if (uiObject.activeInHierarchy == true) uiObject.SetActive(false);
                }
            }
        }
        else
        {
            //disble mouse look
            InputHandler.ModifyCursorState(true, true);

            //turn off pause objects
            if (_activeOnPause.Length > 0)
            {
                foreach (var uiObject in _activeOnPause)
                {
                    if (uiObject.activeInHierarchy == true) uiObject.SetActive(false);
                }
            }

            //turn on permanent objects
            if (_activeAlways.Length > 0)
            {
                foreach (var uiObject in _activeAlways)
                {
                    if (uiObject.activeInHierarchy == false) uiObject.SetActive(true);
                }
            }
        }
    }
}
