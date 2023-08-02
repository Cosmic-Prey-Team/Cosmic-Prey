using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrillWaypoint : MonoBehaviour
{
    private float _destroyTimer = 0f;
    public void Update()
    {
        _destroyTimer += Time.deltaTime;
        if (_destroyTimer > 1f)
        {
            Destroy(gameObject);
        }
    }
}
