using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private float _destroyTimer = 0f;
    public void Update()
    {
        _destroyTimer += Time.deltaTime;
        if (_destroyTimer > 6f)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Whale"))
        {
            Destroy(gameObject);
        }
    }
}
