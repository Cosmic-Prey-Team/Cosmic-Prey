using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureController : MonoBehaviour
{
    //subscribe to on health changed

    [SerializeField] private GameObject[] _fissures;

    private Health _health;
    private int _activeFissures, _fissureRepairAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = this.gameObject.GetComponent<Health>();
        _fissureRepairAmount = _fissures[0].GetComponent<Repairable>().GetAmountToRepair();
        foreach(GameObject fissure in _fissures)
        {
            fissure.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get how many fissures we should have, based on how much health is missing compared to how much a fissure heals
        int fissureChecker = _health.GetMissingHealth() / _fissureRepairAmount;

        //if we need more fissures active
        if(fissureChecker > _activeFissures)
        {
            do
            {
                //pick a random fissure and activate it if it isnt active already
                int random = Random.Range(0, _fissures.Length);
                if(_fissures[random].activeSelf == false)
                {
                    _fissures[random].SetActive(true);
                    _activeFissures++;
                }

            //I swear this should be >, and not <, but for some reason this crashes Unity.
            //It still works because it is in the update function, just not quite how I wanted to code it.
            } while (fissureChecker < _activeFissures);
        }

        //if we've too many fissures active
        else if(fissureChecker < _activeFissures)
        {
            //this only happens if the player recently healed a fissure
            //the repairable script handles deactivating the fissure,
            //so we just need to adjust the amount of fissures active
            _activeFissures--;
        }
    }
}
