using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons.Tornado
{
    public class TornadoWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private TornadoAttackLogic attackLogic;

        public override void Init(WeaponInstance weaponInstance)
        {
            base.Init(weaponInstance);
            attackLogic.Init(this);
        }

        protected override void StartAttack()
        {
            isAttacking = true;
            attackLogic.Attack(instance.damage);
        }

        public override void StopAttack()
        {
            base.StopAttack();
        }
    }

}
