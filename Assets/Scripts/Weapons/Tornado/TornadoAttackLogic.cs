using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons.Tornado
{
    public class TornadoAttackLogic : MonoBehaviour
    {
        [SerializeField] private float spawnRadius = 0.1f;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private GameObject projectilePrefab;
        private Transform currentTarget;
        private float damage;
        private WeaponBehaviour weapon;
        //private PlayerData playerData;
        //private GameSettingsSO gameSettingsSO;

        public void Init(WeaponBehaviour weapon)
        {
            this.weapon = weapon;
            projectilePrefab = weapon.WeaponInstance.projectilePrefab;
            playerTransform = GameManager.Instance.Player.transform;
        }

        public void Start()
        {
            //playerData = DataStorage.Instance.PlayerData;
            //gameSettingsSO = DataStorage.Instance.GameSettings;
        }
        public void Attack(float damage)
        {
            this.damage = (int) damage;

            FindNearestTarget();

            if (currentTarget != null)
            {
                Vector2 direction = (currentTarget.position - transform.position).normalized;

                transform.localPosition = direction * spawnRadius;
                Vector3 offset = direction * spawnRadius;

                Vector2 posToSpawn = transform.position + offset;

                var projectile = Instantiate(projectilePrefab, posToSpawn, Quaternion.identity);
                var tornadoProjectile = projectile.GetComponent<TornadoProjectile>();

                var weaponInstance = weapon.WeaponInstance;

                tornadoProjectile.Init(this.damage, weaponInstance.speed, weaponInstance.attackSpeed, direction, weaponInstance.lifeSpan, weaponInstance.size, weaponInstance.critChance, weaponInstance.critMultiplier, weaponInstance.knockback);
            }

            StopAttack();
        }

        public void StopAttack()
        {
            weapon.StopAttack();
        }

        private void FindNearestTarget()
        {
            GameObject target;
            if (MonstersManager.Instance.TryFindNearestMonsterToPoint(playerTransform, out target))
            {
                currentTarget = target.transform;
            }
            else
            {
                currentTarget = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
