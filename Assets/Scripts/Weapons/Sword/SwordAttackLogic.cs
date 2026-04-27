using Assets.Scripts.Monster;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons.Sword
{
    public class SwordAttackLogic : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Collider2D swordCollider;
        [SerializeField] private Transform playerTransform;
        private Transform currentTarget;
        private float damage;
        private WeaponBehaviour weapon;
        //private PlayerData playerData;
        //private GameSettingsSO gameSettingsSO;

        public void Init(WeaponBehaviour owner)
        {
            weapon = owner;
            playerTransform = GameManager.Instance.Player.transform;
        }

        public void Start()
        {
            //playerData = DataStorage.Instance.PlayerData;
            //gameSettingsSO = DataStorage.Instance.GameSettings;
            //playerTransform = GameManager.Instance.Player.transform;
        }
        public void Attack(float damage, bool isHitCrit)
        {
            this.damage = (int) damage; // пока так, просто хочу целый урон
            animator.speed = weapon.WeaponInstance.attackSpeed;
            transform.localScale = new Vector3(weapon.WeaponInstance.size, weapon.WeaponInstance.size, weapon.WeaponInstance.size);

            FindNearestTarget();

            if (currentTarget != null)
            {
                Vector2 direction = (currentTarget.position - playerTransform.position).normalized;

                transform.localPosition = direction * weapon.WeaponInstance.config.offsetRellativeToParent;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180;
                transform.localRotation = Quaternion.Euler(0, 0, angle);
                spriteRenderer.enabled = true;
                swordCollider.enabled = true;

                animator.SetTrigger("Attack");
            }

        }

        public void StopAttack()
        {
            swordCollider.enabled = false;
            spriteRenderer.enabled = false;
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
                    Vector2 direction = (other.transform.position - transform.position).normalized;
                    enemy.GetComponent<MonsterController>().ApplyKnockback(direction * weapon.WeaponInstance.knockback);
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
