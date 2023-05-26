using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int _playerHealth = 100;

    public int PlayerHealth
    {
        get { return _playerHealth; }
        set { _playerHealth = value; }
    }


    public void PlayerDamage(int damage)
    {
        _playerHealth -= damage;
    }

    public bool PlayerAlive()
    {
        if (_playerHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }





}
