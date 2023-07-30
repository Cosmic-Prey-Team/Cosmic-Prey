using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Weapons
{

    public class AttackController : MonoBehaviour
    { 
        public float Damage => damage;

        public float Multiplier = 1f;

        [Header("Damage")]
        [SerializeField]
        protected float damage = 10f;

        [Header("Timing")]
        [SerializeField, Min(0)]
        protected float attackDuration;

        [SerializeField, Min(0)]
        protected float cooldownDuration;

        [Header("Targeting")]
        public LayerMask targetLayer;

        [Header("Events")]
        public UnityEvent OnAttackStart;
        public UnityEvent OnAttackEnd;
        public UnityEvent OnCooldownStart;
        public UnityEvent OnCooldownEnd;

        protected bool attacking;
        protected bool coolingDown;

        /**
        WeaponCollision[] _collidables;

        protected virtual void Awake()
        {
            ConfigureCollisions();
            DisableCollisions();
        }

        public void ConfigureCollisions()
        {
            _collidables = GetComponentsInChildren<WeaponCollision>();
            foreach (var col in _collidables)
            {
                col.Configure(this, targetLayer);
            }
        }

        /// <summary>
        /// Enables all weapon collisions.
        /// Can be called by <see cref="AnimationEvent"/>s.
        /// </summary>
        public void EnableCollisions()
        {
            foreach (var col in _collidables)
            {
                col.Enable();
            }
        }

        /// <summary>
        /// Disables all weapon collisions.
        /// Can be called by <see cref="AnimationEvent"/>s.
        /// </summary>
        public void DisableCollisions()
        {
            foreach (var col in _collidables)
            {
                col.Disable();
            }
        }
        **/
        public bool AttemptAttack(bool loop = false, System.Action AfterAttack = null, System.Action AfterCooldown = null)
        {
            bool canAttack = CanAttack();
            if (!canAttack) return false;

            Attack(AfterAttack, AfterCooldown);
            return true;
        }

        public virtual bool CanAttack()
        {
            
        if (attacking || coolingDown)
        {
            return false;
        }

        return true;
        }

        //protected abstract void OnAttack();

        void Attack(System.Action AfterAttack, System.Action AfterCooldown)
        {
            //OnAttack();
            //OnAction?.Invoke(ActionType.Attack);
            StartCoroutine(AttackDelay(AfterAttack, AfterCooldown));
        }

        IEnumerator AttackDelay(System.Action AfterAttack, System.Action AfterCooldown)
        {
            // perform attack
            attacking = true;
            OnAttackStart?.Invoke();

            yield return new WaitForSeconds(attackDuration);
            AfterAttack?.Invoke();
            OnAttackEnd?.Invoke();

            // wait for cooldown
            attacking = false;
            coolingDown = true;
            OnCooldownStart?.Invoke();

            yield return new WaitForSeconds(cooldownDuration);
            AfterCooldown?.Invoke();
            coolingDown = false;
            OnCooldownEnd?.Invoke();

            yield break;
        }
    }
}