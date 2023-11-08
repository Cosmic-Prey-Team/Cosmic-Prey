using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureVisuals : MonoBehaviour
{
    [SerializeField] Transform _panel;
    [SerializeField] Transform _fissure;
    [SerializeField] GameObject _smokeEffect;

    private GameObject _smoke; 

    Collider _collider;

    private bool _isActive;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
    private void Start()
    {
        if(_panel != null) _panel.gameObject.SetActive(false);

        //disable collider visual
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public bool GetIsActive()
    {
        return _isActive;
    }
    public void EnablePanel()
    {
        //activate panel visual on isRepairing
        _panel.gameObject.SetActive(true);
        float randRotation = UnityEngine.Random.Range(0, 90f);
        float randScale = UnityEngine.Random.Range(0.9f, 1.2f);
        _panel.localRotation = Quaternion.Euler(randRotation, _panel.localRotation.eulerAngles.y, _panel.localRotation.eulerAngles.z);
        _panel.localScale = Vector3.one * randScale;
        Debug.Log("Enable panel");
    }
    public void Activate()
    {
        //enable collider
        _collider.enabled = true;
        //enable fissure visual
        _fissure.gameObject.SetActive(true);
        //start smoke effect
        if (_smoke != null)
            _smoke = Instantiate(_smoke, transform.position, Quaternion.identity);
        else
            Debug.LogWarning("_smoke is not assigned");
        //disable panel
        _panel.gameObject.SetActive(false);

        _isActive = true;

        Debug.Log("Activate Fissure");
    }
    public void Deactivate()
    {
        //disable collider
        _collider.enabled = false;
        //disable fissure visual
        _fissure.gameObject.SetActive(false);
        //disable smoke
        Destroy(_smoke);

        _isActive = false;
        Debug.Log("Deactivate Fissure");
    }
}
