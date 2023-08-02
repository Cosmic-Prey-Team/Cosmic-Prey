using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThrottle : MonoBehaviour
{

    private ShipController _shipController;

    // Start is called before the first frame update
    void Start()
    {
        _shipController = FindObjectOfType<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        //throttle rotation range is (-30, 90), with -30 being max speed and 90 being min speed (i dont know why its backwards but it is)
        float range = 120;

        //start at lowest point
        float rotation = 90;

        //if the ship is moving
        if (_shipController.GetSpeed() != 0)
        {
            //calculate the rotation value we should have
            rotation = range / (_shipController.GetMaxSpeed() / _shipController.GetSpeed()) - 30;

            //since 90 is the lowest point, not highest, we need to flip the number in its range
            //this is done with (max# + min#) - num, or (90 + (-30)) - rotation
            rotation = 60 - rotation;
        }
        // no need for else, since rotation is still 90 if we didn't do the if statement


        //set the rotation
        transform.localRotation = Quaternion.Euler(new Vector3(rotation, 0f, 0f));

    }
}
