using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Monster
{
    internal class MonsterController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private MonsterSettingsSO monsterSettingsSO;
        private float speed;
        private float damage;
        private float damageInterval;
        private float distanceToStop;

        [SerializeField] private Rigidbody2D rgbody;
        [SerializeField] private bool rotateSpriteToTarget;
        [SerializeField] private bool inverseSpriteRotation;

        private bool playerInRange = false;
        private float damageTimer = 0f;
        private PlayerController player;
        private Animator animator;
        private float speedMultiplier;
        private SpriteRenderer spriteRenderer;
        private bool isKnockedBack;

        private void Start()
        {
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
            speed = monsterSettingsSO.Speed;
            damage = monsterSettingsSO.Damage;
            damageInterval = monsterSettingsSO.DamageInterval;
            distanceToStop = monsterSettingsSO.DistanceToStop;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (isKnockedBack) 
                return;

            if (target == null)
                return;


            Vector2 direction = CalculateDirection();

            if (CalculateDistance() >= distanceToStop)
            {
                Vector2 newPosition = rgbody.position + direction * speed * speedMultiplier * Time.fixedDeltaTime;
                rgbody.MovePosition(newPosition);
                animator.SetBool("isMoving", true);

                if (rotateSpriteToTarget)
                {
                    if (direction.x > 0)
                    {
                        spriteRenderer.flipX = !inverseSpriteRotation ? true : false;
                    }
                    else if (direction.x < 0)
                    {
                        spriteRenderer.flipX = !inverseSpriteRotation ? false : true;
                    }
                }
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        private void Update()
        {
            if (!playerInRange || player == null)
                return;

            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                player.TakeDamage(damage);
                damageTimer = 0f;
            }
        }

        private Vector2 CalculateDirection()
        {
            var targetPosition = target.transform.position;
            var diff = targetPosition - transform.position;
            return new Vector2(diff.x, diff.y).normalized;
        }

        private float CalculateDistance()
        {
            return Vector2.Distance(target.transform.position, transform.position);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            this.speedMultiplier = speedMultiplier;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (player != null)
                {
                    playerInRange = true;
                    damageTimer = damageInterval;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInRange = false;
                damageTimer = 0f;
            }
        }

        public void ApplyKnockback(Vector3 force)
        {
            if (force.magnitude != 0)
            isKnockedBack = true;
            rgbody.linearVelocity = Vector3.zero;
            rgbody.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(KnockbackEnd());
        }

        IEnumerator KnockbackEnd()
        {
            yield return new WaitForSeconds(0.125f);
            isKnockedBack = false;
        }
    }
}
