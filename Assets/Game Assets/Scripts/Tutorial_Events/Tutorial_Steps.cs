using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Steps : MonoBehaviour
{
    public int Order;
    public GameObject[] QueenPopups;
    public GameObject[] QuestPopups;

    // Start is called before the first frame update
    void Awake()
    {
        GameTutorial_Manager.Instance.Tutorial.Add(this);
    }
    public virtual void CheckIf_Happending()
    {

    }
}
