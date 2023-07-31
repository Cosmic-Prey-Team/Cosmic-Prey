using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [Header("Transforms")]
    [Tooltip("Object to be tracked by the radar.")]
    [SerializeField] Transform _objectToTrack;
    [Tooltip("The icon on the display representing the object being tracked.")]
    [SerializeField] Transform _dot;

    [Header("Radar Properties")]
    [Tooltip("Detection range of the radar.")]
    [SerializeField] float _maxRange = 100f;
    [Tooltip("Radius of the radar's display.")]
    [SerializeField] float _displayRadius = 0.115f;
    
    private float _currentDist;
    private ShipController _ship;

    private void Awake()
    {
        _ship = FindObjectOfType<ShipController>();
    }
    private void Update()
    {
        if (_objectToTrack == null || _dot == null) return;

        Vector3 objPos = new Vector3(_objectToTrack.position.x, transform.position.y, _objectToTrack.position.z);
        Vector3 heading = transform.position - objPos;
        float dist = heading.magnitude;

        if (_currentDist != dist)
        {
            if(dist <= _maxRange)
            {
                if (_dot.gameObject.activeInHierarchy == false)
                    _dot.gameObject.SetActive(true);

                //do fraction of distance
                float screenDist = (_displayRadius * dist) / _maxRange;

                //get direction
                var direction = heading / dist;

                Vector3 adjDirection = new Vector3(-direction.x, -direction.z, direction.y);

                //move dot by screenDist in correct direction
                _dot.localPosition = screenDist * adjDirection;

                _currentDist = dist;

                //rotate display to match ship orientation
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,
                    transform.localRotation.eulerAngles.y, _ship.transform.rotation.eulerAngles.y);
            }
            else
            {
                if(_dot.gameObject.activeInHierarchy == true)
                    _dot.gameObject.SetActive(false);
            }
            
        }
    }
}
