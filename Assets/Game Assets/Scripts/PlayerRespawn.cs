using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRespawn : MonoBehaviour
{
    [Tooltip("makes the X button kill you, for testing")]
    [SerializeField] private bool _debug;
    [Tooltip("The Whale game object")]
    [SerializeField] private GameObject _whale;
    [Tooltip("The Ship game object")]
    [SerializeField] private GameObject _ship;
    [Tooltip("The MovePlayerWithScript script on the ship")]
    [SerializeField] private MovePlayerWithShip _movePlayerWithShip;

    [Header("Respawn Properties")]
    [Tooltip("the minimum distance from the whale that the player can spawn at")]
    [SerializeField] private float _minDistanceFromWhale;
    [Tooltip("the maximum distance from the whale that the player can spawn at")]
    [SerializeField] private float _maxDistanceFromWhale;

    private Health _health;
    private Health _shipHealth;
    private PlayerState _playerState;

    [SerializeField] private WorldManager _world;
    private float _worldX;
    private float _worldY;
    private float _worldZ;

    private void Awake()
    {
        _health = gameObject.GetComponent<Health>();
        _shipHealth = _ship.GetComponent<Health>();
        _worldX = _world.GridWidth * _world.PointDistance;
        _worldY = _world.GridHeight * _world.PointDistance;
        _worldZ = _world.GridLength * _world.PointDistance;
    }

    public void Respawn()
    {
        float angle;
        float distance;
        bool onShip = _movePlayerWithShip.onShip;

        angle = Random.Range(0f, 360f);
        distance = Random.Range(_minDistanceFromWhale, _maxDistanceFromWhale);
        
        //if the player was on the ship
        if (onShip)
        {
            //the player is a child of the ship, move the ship and the player moves with it
            _ship.transform.Rotate(new Vector3(0f, angle, 0f));
            _ship.transform.position = new Vector3(Mathf.Clamp(_whale.transform.position.x + distance, _whale.transform.position.x + distance, _worldX*0.90f), Mathf.Clamp(_whale.transform.position.y + distance, _whale.transform.position.y + distance, _worldY * 0.90f), Mathf.Clamp(_whale.transform.position.z + distance, _whale.transform.position.z + distance, _worldZ * 0.90f));
        }
        else
        {
            //move the player to the chosen position
            transform.Rotate(new Vector3(0f, angle, 0f));
            transform.Translate(new Vector3(_whale.transform.position.x + distance, 0f, _whale.transform.position.z + distance));
            //move the ship to the same spot, -1 in the y axis so the player spawns on top of the ship
            _ship.transform.position = new Vector3(transform.position.x, transform.position.y - 4f, transform.position.z);
        }

        _health.Heal(_health.GetMissingHealth());
        _health._dead = false;
        _shipHealth.Heal(_shipHealth.GetMissingHealth());
        _whale.GetComponent<AIAgent>().stateMachine.ChangeState(AIStateID.WhaleWander);
    }

    void Update()
    {
        //Kill button for testing. Delete later
        if(_debug && Input.GetKeyDown(KeyCode.X))
        {
            _health.Damage(100);
        }
    }
}
