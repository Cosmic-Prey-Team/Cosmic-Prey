using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private float HealthPercentage;
    private int maxHealth = 100;
    private int playerHealth;

    [SerializeField]
    private Slider playerHealthBar1;
    [SerializeField]
    private Slider playerHealthBar2;
    [SerializeField]
    private Slider playerHealthBar3;

    [SerializeField]
    private Slider whaleHealth;

    [SerializeField]
    private Slider shipHealth;

    private void Start()
    {
        playerHealth = 100;
        updatePlayerHealthBar(playerHealth);
        whaleHealth.value = maxHealth;
        shipHealth.value = maxHealth;
    }

    public void updatePlayerHealthBar(float health)
    {
        
        HealthPercentage = (health / maxHealth);
        if (HealthPercentage >= .66)
        {
            playerHealthBar3.value = health % 66;
            playerHealthBar2.value = 33;
            playerHealthBar1.value = 33;
        }
        else if (HealthPercentage > .33) {
            playerHealthBar3.value = 0;
            playerHealthBar2.value = health % 33;
            playerHealthBar1.value = 33;
        }
        else
        {
            playerHealthBar3.value = 0;
            playerHealthBar2.value = 0;
            playerHealthBar1.value = health;
        }
    }

    public void updateWhaleHealthBar(int health)
    {
        whaleHealth.value = health;
    }

    public void updateShipHealthBar(int health)
    {
        shipHealth.value = health;
    }
}
