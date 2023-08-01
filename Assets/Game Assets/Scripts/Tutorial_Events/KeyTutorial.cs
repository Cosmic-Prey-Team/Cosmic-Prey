using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : Tutorial_Steps
{
    public List<string> Keysinput = new List<string>();


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
        if (Keysinput.Count == 0)
            GameTutorial_Manager.Instace.TutorialIncrement();
    }
}
