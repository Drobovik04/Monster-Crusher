using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Weapons.Sword
{
    public class SwordWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private SwordAttackLogic attackLogic;

        public override void Init(WeaponInstance weaponInstance)
        {
            base.Init(weaponInstance);
            attackLogic.Init(this);
        }

        protected override void StartAttack()
        {
            isAttacking = true;
            if (UnityEngine.Random.Range(0f, 100f) <= Math.Clamp(instance.critChance, 0, 100))
            {
                attackLogic.Attack(instance.critMultiplier * instance.damage, true);
            }
            else
            {
                attackLogic.Attack(instance.damage, false);
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();
        }
    }

}
