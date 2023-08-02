using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShipHelm : MonoBehaviour
{
    [SerializeField] private Animator _shipHandleAnim;
    [SerializeField] private Animator _shipWheelAnim;

    private float _pause;
    private bool _left, _right;
    private PlayerState _playerState;
    private ShipInput _input;
    // Start is called before the first frame update
    void Start()
    {
        _playerState = FindObjectOfType<PlayerState>();
        _input = _playerState.GetComponent<ShipInput>();
    }

    // Update is called once per frame
    void Update()
    {
        TurnWheel();
    }

    void TurnWheel()
    {
        if (_input.turn < 0 && _right == false)
        {
            _shipHandleAnim.Play("HandleTurnLeft");
            _shipWheelAnim.Play("WheelTurnLeft");
            _left = true;
        }
        else if (_input.turn > 0 && _left == false)
        {
            _shipHandleAnim.Play("HandleTurnRight");
            _shipWheelAnim.Play("WheelTurnRight");
            _right = true;
        }
        else
        {
            if (_left)
            {
                _shipHandleAnim.Play("HandleReturnFromLeft");
                _shipWheelAnim.Play("WheelReturnFromLeft");

                if (_pause == 0)
                    _pause = Time.time + _shipHandleAnim.GetCurrentAnimatorStateInfo(0).length;
                if (Time.time > _pause)
                {
                    _left = false;
                    _pause = 0;
                }
            }
            if (_right)
            {
                _shipHandleAnim.Play("HandleReturnFromRight");
                _shipWheelAnim.Play("WheelReturnFromRight");

                if (_pause == 0)
                    _pause = Time.time + _shipHandleAnim.GetCurrentAnimatorStateInfo(0).length;
                if (Time.time > _pause)
                {
                    _right = false;
                    _pause = 0;
                }
            }
        }
    }
}
