using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SMBEventCurator : MonoBehaviour
{

    [SerializeField] private bool _debug = false;
    [SerializeField] private UnityEvent<string> _event = new UnityEvent<string>();
    public UnityEvent<string> Event { get => _event; }
    private void Awake()
    {
        _event.AddListener(OnSMBEvent);
    }

    private void OnSMBEvent(string eventName)
    {
        if (_debug)
        {
            Debug.Log(eventName);
        }
    }
}
