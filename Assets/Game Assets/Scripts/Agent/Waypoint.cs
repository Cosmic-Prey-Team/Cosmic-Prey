using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //private float _destroyTimer = 0f;
    /**
    public void Update()
    {
        
        _destroyTimer += Time.deltaTime;
        if (_destroyTimer > 6f)
        {
            Destroy(gameObject);
        }
        
}
    **/
    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Whale"))
        //{
            Vector3 direction = (gameObject.transform.position - other.gameObject.transform.position).normalized;
            float angleY = Random.Range(-30, 30);
            float angleZ = Random.Range(-30, 30);
            float distance = Random.Range(8, 20);
            direction = Quaternion.Euler(0, angleY / 0.5f, angleZ / 1.5f) * direction * distance;
            gameObject.transform.position = direction;
        //}
    }
}
