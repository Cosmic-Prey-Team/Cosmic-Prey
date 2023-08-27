using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVoiceOver : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    void Start()
    {
        QueenVA_Manager.Instance.PlayQueenVA_Line(_clip);
    }

}
