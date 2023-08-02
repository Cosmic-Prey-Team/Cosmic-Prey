using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorial_Manager : MonoBehaviour
{
    //Player progress of the tutorial script
    public List<Tutorial_Steps> Tutorials = new List<Tutorial_Steps>();

    public Text expTexts;

    private Tutorial_Steps currentTutorial;

    //Getter
    public static GameTutorial_Manager instance;
    public static GameTutorial_Manager Instace
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameTutorial_Manager>();
            }

            if (instance == null)
            {
                Debug.Log("Ther is no GameTutorial_Manager");
            }
            return instance;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        SetNextTutorial(0);
    }

    void Update()
    {
        if (currentTutorial) 
            currentTutorial.CheckIf_Happending();
    }
    public Tutorial_Steps GetTutorialByOrder(int order)
    {
        for (int step = 0; step < Tutorials.Count; step++)
        {
            if (Tutorials[step].Order == order)
                return Tutorials[step];
        }

        //Tutorial ends or skipped
        return null;
    }

    //Next step of the tutorial
    public void TutorialIncrement()
    {
        SetNextTutorial(currentTutorial.Order + 1);
    }
    
    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);
  
        //Last step, Queens' final quote    
        if (!currentTutorial) 
        {
            CompletedAllTuturials();
            return;
        }
        //Next player quest/task
        expTexts.text = currentTutorial.QueenPopups;
    }

    public void CompletedAllTuturials()
    {
        expTexts.text = "Queen: Well, moment of truth… Let's hope those hull parts hold. Go get that whale! ";
    }
}