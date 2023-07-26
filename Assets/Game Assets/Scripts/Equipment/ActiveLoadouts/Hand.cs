using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public Image ObjectwithImage;       //Gray scale icon
    public Sprite spriteToChangeItTo;   //Active icon

    // Update is called once per frame
    public void Swap(bool flag)
    {
        if (flag)
            ObjectwithImage.sprite = spriteToChangeItTo;
    }
}
