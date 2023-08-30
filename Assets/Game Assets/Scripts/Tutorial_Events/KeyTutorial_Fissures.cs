using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial_Fissures : Tutorial_Steps
{
    public Repairable _repairProgress;

    public List<string> Keysinput = new List<string>();

    public static int _RepairedFissures = 0;

    void Awake()
    {
  
    }
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

        if (Keysinput.Count == 0 && _RepairedFissures >= 2) 
        {
            _RepairedFissures = 0;
            GameTutorial_Manager.Instace.TutorialIncrement();
        }
    }
    public void repairProgressTransfer()
    {
        _RepairedFissures++;
    }

}
