using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : Tutorial_Steps
{
    private bool isCurrentTutorial = false;
    public Transform HitTransform;
    public override void CheckIf_Happending()
    {
        isCurrentTutorial = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isCurrentTutorial)
            return;

        //The player reaches a collision space
        if (other.transform == HitTransform)
            isCurrentTutorial = false;
    }
}
