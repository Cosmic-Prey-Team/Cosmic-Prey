using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenVA_Manager : MonoBehaviour
{
    public static QueenVA_Manager Instance;
    [SerializeField] private AudioSource _effectsSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlayQueenVA_Line(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.LogError("No audio clip found.");
            return;
        }

        if (_effectsSource != null)
            _effectsSource.PlayOneShot(clip);
        else
            Debug.LogError("_effectsSource == null");
    }
}