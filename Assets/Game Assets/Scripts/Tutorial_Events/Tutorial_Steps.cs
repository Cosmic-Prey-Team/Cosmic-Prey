using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial_Steps : MonoBehaviour
{
    public int _Order;

    //Min and max amount of lines
    [TextArea(3,25)]
    public string _QueenPopups,
                  _Questopups;


    // Start is called before the first frame update
    void Awake()
    {
        GameTutorial_Manager.instance.Tutorials.Add(this);
    }
    public virtual void CheckIf_Happending()
    {

    }

    public void DisplayQuotes_Queen(int phrase)
    {
        //print(QueenPopups[phrase]);
    }
    public void DisplayQuotes_Quest(int phrase)
    {
        //Print out quest line
    }
}