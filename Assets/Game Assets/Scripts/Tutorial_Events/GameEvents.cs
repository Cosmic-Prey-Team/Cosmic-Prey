using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorial_Manager : MonoBehaviour
{
    //Player progress of the tutorial script
    public List<Tutorial> Tutorial = new List<Tutorial>();
    
    //Getter
    public static GameTutorial_Manager instance;
    public static GameTutorial_Manager instance
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

    public GameTutorial_Manager currentStep;
    private Tutorial_Steps currentTutorial;

    // Start is called before the first frame update
    void Start()
    {
        SetNextTutorial(0);
    }

    void Update()
    {
        if(currentTutorial)
    }
    public GameTutorial_Manager GetTutorialByOrder(int order)
    {
        for (int step = 0; step < Tutorial.Count; step++)
        {
            if (Tutorial[step].order == order)
                return Tutorial[step];
        }

        //Tutorial ends or skipped
        return null;
    }

    public void TutorialIncrement()
    {
        SetNextTutorial(currentStep.GetTutorialByOrder +1)
    }
    //Next step of the tutorial
    public void SetNextTutorial(int currentOrder)
    {
        currentStep = GetTutorialByOrder(currentOrder);
  
        //Last step, Queens' final quote    
        if (!currentStep) 
        {
            CompletedAllTuturials();
            return;
        }
        //Next player quest/task
    }

    public void CompletedAllTuturials()
    {
        expText.text = "Queen: Well, moment of truth… Let's hope those hull parts hold. Go get that whale!";
    }

    /*
    
    //1st goal in tutorial 
    public event Action GotoAsteriod;
    public void GotoAsteriod_Trigger()
    {
        if (GotoAsteriod != null)
        {
            /*Queen speaks
             * Call/display player movement controls
             
        }
    }
/*
    //When the player reaches the asteroid
    public event Action AsteriodReached;
    public void AsteriodReached_Trigger() 
    {
        if (AsteriodReached != null)
        {
            /*Queen talks to player
             * Call/display switching loadout controls controls of the ship
             Present a ore amount goal
             
        }

         Quenn's 1st line to player
         Call/display movement controls of the ship
         
    }

    //Switching loadout to drill and learning about mining
    public event Action AsteriodMining;
    public void AsteriodMining_Trigger()
    {
        if (AsteriodMining != null)
        {
            /* Queen talks to player
             * Indcate goal of ore is collected
             * Next step is crafting at Processor
        }
        
    }
/*
    //Switching loadout to drill and learning about mining
    public event Action GotoProcessor;
    public void GotoProcessor_Trigger()
    {
        if (GotoProcessor != null)
        {
             Queen talks to player
             * Indcate goal of ore to collect
             * Next step is crafting at Processor
        }

    }
/*
    //Learing about crafting and drag+drop implementation
    public event Action CraftObjects;
    public void CraftObjects_Trigger()
    {
        if (CraftObjects != null)
        {
             Queen talks to player
             * Indcate goal of broken areas are repaired
             * Next step is steering the ship
        }

    }

    //Switching loadout to repair tool and learning about repairing
    public event Action RepairShip;
    public void RepairShip_Trigger()
    {
        if (RepairShip != null)
        {
             Queen talks to player
             * Indcate goal of broken areas are repaired
             * Next step is steering the ship
        }

    }
    //Ship steering and end of tutorial
    public event Action ShipDriving;
    public void ShipDriving_Trigger()
    {
        if (ShipDriving != null)
        {
             Queen talks to player
             Indcate goal of broken areas are repaired
              Next step is steering the ship*/
     /*   }

    }
      

