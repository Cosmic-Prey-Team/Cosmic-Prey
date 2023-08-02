using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeMovement : MonoBehaviour
{
    //assuming the minimum height is the maximum height multiplied by -1
    [Tooltip("The maximum height the ship can reach before being out of bounds")]
    [SerializeField] private float _maxHeight;
    [Tooltip("The number of degrees the needle can rotate from neutral")]
    [SerializeField] private float _rotateDistance;
    [Tooltip("The ship gameobject")]
    [SerializeField] private GameObject _ship;

    // Update is called once per frame
    void Update()
    {
        float currentHeight = _ship.transform.position.y;
        float rotation = _rotateDistance / (_maxHeight / currentHeight);

        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -rotation));
    }
}
