using Assets.Scripts.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons.Tornado
{
    public class TornadoProjectile : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float speed;
        [SerializeField] private float attackSpeed;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float lifeSpan;
        [SerializeField] private float size;
        [SerializeField] private float critChance;
        [SerializeField] private float critMultiplier;
        [SerializeField] private float knockback;
        [SerializeField] private Collider2D tornadoCollider;
        [SerializeField] private Rigidbody2D rgbody;
        [SerializeField] private Animator animator;

        private float attackCooldown;
        private float elapsedTimeSpan;
        private bool isInitialized;
        private bool colliderActiveThisFrame;

        public void Init(float damage, float speed, float attackSpeed, Vector2 direction, float lifeSpan, float size, float critChance, float critMultiplier, float knockback)
        {
            this.damage = damage;
            this.speed = speed;
            this.attackSpeed = attackSpeed;
            this.direction = direction;
            this.lifeSpan = lifeSpan;
            this.size = size;
            this.critChance = critChance;
            this.critMultiplier = critMultiplier;
            this.knockback = knockback;

            animator.speed = attackSpeed;
            transform.localScale.Set(size, size, size);
            isInitialized = true;
        }


        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            attackCooldown -= Time.deltaTime;
            elapsedTimeSpan += Time.deltaTime;

            if (attackCooldown <= 0)
            {
                tornadoCollider.enabled = true;
                colliderActiveThisFrame = true;
                attackCooldown = 1f / attackSpeed;
            }

            if (elapsedTimeSpan >= lifeSpan) 
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (!isInitialized)
            {
                return;
            }

            if (colliderActiveThisFrame)
            {
                colliderActiveThisFrame = false;
            }

            Vector2 newPosition = rgbody.position + direction * speed * Time.fixedDeltaTime;
            rgbody.MovePosition(newPosition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null)
                {
                    Vector2 direction = (other.transform.position - transform.position).normalized;
                    enemy.GetComponent<MonsterController>().ApplyKnockback(direction * knockback);
                    if (UnityEngine.Random.Range(0, 100f) <= Math.Clamp(critChance, 0, 100))
                    {
                        enemy.TakeDamage((int) critMultiplier * damage, true);
                    }
                    else
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
