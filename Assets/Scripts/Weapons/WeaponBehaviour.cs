using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        protected WeaponInstance instance;
        protected float attackCooldown;
        protected bool isAttacking;

        public WeaponInstance WeaponInstance => instance;

        public virtual void Init(WeaponInstance weaponInstance)
        {
            instance = weaponInstance;
            attackCooldown = 1f / instance.attackSpeed;
        }

        protected virtual void Update()
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0)
            {
                StartAttack();
                attackCooldown = 1f / instance.attackSpeed;
            }
        }

        protected abstract void StartAttack();

        public virtual void StopAttack()
        {
            isAttacking = false;
        }
    }
}
