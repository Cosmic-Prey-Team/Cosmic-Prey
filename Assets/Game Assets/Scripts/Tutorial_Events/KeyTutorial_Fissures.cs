using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial_Fissures : Tutorial_Steps
{
    public List<string> Keysinput = new List<string>();

    //public Repairable _repairProgress;

    private int _RepairedFissures;
   

    /*void Awake()
    {
        _repairProgress = FindObjectOfType<Repairable>();
        _repairProgress.RepairedQuest += repairProgressTransfer;
    }*/
    public override void CheckIf_Happending()
    {
        for (int index = 0; index < Keysinput.Count; index++)
        {
            if (Input.inputString.Contains(Keysinput[index]))
            {
                Keysinput.RemoveAt(index);
                break;
            }
        }
       
        if (Keysinput.Count == 0 && _RepairedFissures >= 4)
            GameTutorial_Manager.Instace.TutorialIncrement();
        else if (_RepairedFissures < 4)
        {
            int _remainingFissurse = 4 - _RepairedFissures;
            DisplayQuotes_Quest($"There are {_remainingFissurse} left to fix!");
        }  
    }

    public void FissuresFixed() 
    {
       _RepairedFissures++;
        return;
    }
}
