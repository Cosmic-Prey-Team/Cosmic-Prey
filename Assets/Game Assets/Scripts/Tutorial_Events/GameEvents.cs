using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public GameObject[] QueenPopups;
    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }

    //1st goal in tutorial 
    public event Action GotoAsteriod;
    public void GotoAsteriod_Trigger()
    {
        if (GotoAsteriod != null)
        {
            /*Queen speaks
             * Call/display player movement controls
             * */
        }
    }

    //When the player reaches the asteroid
    public event Action AsteriodReached;
    public void AsteriodReached_Trigger() 
    {
        if (AsteriodReached != null)
        {
            /*Queen talks to player
             * Call/display switching loadout controls controls of the ship
             * Present a ore amount goal
             */
        }

        /* Quenn's 1st line to player
         * Call/display movement controls of the ship
         */
    }

    //Switching loadout to drill and learning about mining
    public event Action AsteriodMining;
    public void AsteriodMining_Trigger()
    {
        if (AsteriodMining != null)
        {
            /* Queen talks to player
             * Indcate goal of ore is collected
             * Next step is crafting at Processor*/
        }
        
    }

    //Switching loadout to drill and learning about mining
    public event Action GotoProcessor;
    public void GotoProcessor_Trigger()
    {
        if (GotoProcessor_Trigger != null)
        {
            /* Queen talks to player
             * Indcate goal of ore to collect
             * Next step is crafting at Processor*/
        }

    }

    //Learing about crafting and drag+drop implementation
    public event Action CraftObjects;
    public void CraftObjects_Trigger()
    {
        if (CraftObjects != null)
        {
            /* Queen talks to player
             * Indcate goal of broken areas are repaired
             * Next step is steering the ship*/
        }

    }

    //Switching loadout to repair tool and learning about repairing
    public event Action RepairShip;
    public void RepairShip_Trigger()
    {
        if (CraftingParts != null)
        {
            /* Queen talks to player
             * Indcate goal of broken areas are repaired
             * Next step is steering the ship*/
        }

    }

    //Ship steering and end of tutorial
    public event Action ShipDriving;
    public void ShipDriving_Trigger()
    {
        if (CraftingParts != null)
        {
            /* Queen talks to player
             * Indcate goal of broken areas are repaired
             * Next step is steering the ship*/
        }

    }
}
