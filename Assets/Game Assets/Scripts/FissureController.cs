using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FissureController : MonoBehaviour
{
    //[Tooltip("")]
    [Tooltip("allows you to damage the ship by pressing c")]
    [SerializeField] bool _debug = false;
    [Tooltip("list of all the fissure spots on the ship")]
    [SerializeField] private GameObject[] _fissures;
    [Tooltip("how much health the ship heals each second when there are zero fissures active")]
    [SerializeField] private int _healthRegenRate;
    private float _healthRegenProgress = 0;

    private Health _health;
    private bool _canRegenHealth;
    private int _activeFissures = 0;
    private int _fissureRepairAmount;

    private void Awake()
    {
        _health = gameObject.GetComponent<Health>();
        _health.OnHealthChanged += FissureChecker;

    }
    // Start is called before the first frame update
    void Start()
    {
        float repairAmount = (float)_health.GetHealth() / (float)_fissures.Length;
        _fissureRepairAmount = (int)repairAmount;

        foreach (GameObject fissure in _fissures)
        {
            fissure.GetComponent<Repairable>().SetAmountToRepair(_fissureRepairAmount + 1);
            fissure.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame && _debug)
        {
            _health.Damage(20);
        }

        //might not be needed
        if (_activeFissures == 0) _canRegenHealth = true;
        else _canRegenHealth = false;

        //health regen if no active fissures
        if(_health.GetMissingHealth() != 0 && _canRegenHealth)
        {
            _healthRegenProgress -= Time.deltaTime;

            if(_healthRegenProgress <= 0)
            {
                _health.Heal(_healthRegenRate);
                _healthRegenProgress = 1; //1 because health regens every second
            }
        }
    }

    public void FissureChecker()
    {
        //Debug.Log("Health Change");

        int fissuresRequired = _health.GetMissingHealth() / _fissureRepairAmount;

        while(_activeFissures < fissuresRequired)
        {
            int random = Random.Range(0, _fissures.Length);
            if (_fissures[random].activeSelf == false)
            {
                _fissures[random].SetActive(true);
                _activeFissures++;
            }
        }

        if(fissuresRequired < _activeFissures)
        {
            _activeFissures--;
        }
    }
}
