using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    [Tooltip("The asteroid prefab to spawn")]
    [SerializeField] private GameObject _asteroid;

    [Header("Asteroid Spawning Properties")]
    [Tooltip("The minimum distance between asteroids")]
    [SerializeField] private float _minDistanceApart = 50;

    [Tooltip("the amount of asteroids to spawn in the map")]
    [SerializeField] private int _numAsteroidsToSpawn = 10;

    [Tooltip("the size of the map")]
    [SerializeField] private float _mapSize = 500;


    private Vector3 FindNewPosition()
    {
        Vector3 Position;
        Collider[] _collision;

        do
        {
            //generates a random position within the map
            Position = new Vector3(Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize));

            //checks if anything exists within a range of _minDistanceApart
            //this means no asteroids will spawn within a set distance from the ship and whale's spawning position
            _collision = Physics.OverlapSphere(Position, _minDistanceApart);

            //repeat until we find a position far enough from everything
        } while (_collision.Length > 0);

        //if it is far enough from everything, return the generated position
        return Position;
    }

    void Start()
    {
        //create parent object for asteroids
        GameObject parent = new GameObject("Asteroids");

        //spawn asteroids
        for (int i = 0; i < _numAsteroidsToSpawn; i++)
        {
            Vector3 Position = FindNewPosition();
            GameObject asteroid = Instantiate(_asteroid, Position, transform.rotation);
            asteroid.transform.SetParent(parent.transform);
        }
    }
}
