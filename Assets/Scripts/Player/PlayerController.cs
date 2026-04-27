using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Upgrades;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.UI.Upgrades;
using Assets.Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float currentHealth;
        [SerializeField] private float collisionOffset;
        [SerializeField] private ContactFilter2D movementFilter;
        [SerializeField] private int currentExp;
        [SerializeField] private int expToLevelUp;
        [SerializeField] private int currentLevel = 0;
        [SerializeField] private int upgradesToShow;
        [SerializeField] private UpgradeGenerator generator;

        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private PolygonCollider2D playerCollider;
        private Vector2 moveInput;
        private bool canMove = true;
        private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        private GameSettingsSO gameSettingsSO;
        private HealthBar healthBar;
        private float speed;
        private float maxHealth;
        private ExperienceBar experienceBar;
        private TMP_Text levelText;
        private PlayerWeapons playerWeapons;
        
        private Vector2[][] originalPaths;
        private bool facingLeft = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerCollider = GetComponent<PolygonCollider2D>();
            gameSettingsSO = DataStorage.Instance.GameSettings;
            var UI = GameObject.FindGameObjectWithTag("UI");
            healthBar = UI.GetComponentInChildren<HealthBar>();
            currentHealth = gameSettingsSO.BasicHealth;
            maxHealth = currentHealth;
            speed = gameSettingsSO.BasicMoveSpeed;
            healthBar.Initialize(maxHealth);
            experienceBar = UI.GetComponentInChildren<ExperienceBar>();
            levelText = UI.transform.Find("Level").GetComponent<TMP_Text>();
            playerWeapons = GetComponent<PlayerWeapons>();

            originalPaths = new Vector2[playerCollider.pathCount][];
            for (int i = 0; i < playerCollider.pathCount; i++)
                originalPaths[i] = (Vector2[])playerCollider.GetPath(i).Clone();
        }

        private void Start()
        {
            expToLevelUp = gameSettingsSO.BasicNeededExpForFirstLevel;
            playerWeapons.AddWeapon(DataStorage.Instance.WeaponsSettings.Find(x => x.weaponName == "Sword"));
            playerWeapons.AddWeapon(DataStorage.Instance.WeaponsSettings.Find(x => x.weaponName == "Tornado"));
        }

        public void OnMove(InputValue movementValue)
        {
            moveInput = movementValue.Get<Vector2>().normalized;
        }

        public void OnAttack()
        {
            //animator.SetTrigger("Attack");
            //swordAttackLogic.Attack();
            Debug.Log("Атака игрока по ЛКМ (пока пусть будет)");
        }

        public void LockMove()
        {
            canMove = false;
        }

        public void UnlockMove()
        {
            canMove = true;
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                if (moveInput != Vector2.zero)
                {
                    bool success = TryMove(moveInput);

                    if (!success)
                    {
                        success = TryMove(new Vector2(moveInput.x, 0));
                    }

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, moveInput.y));
                    }

                    animator.SetBool("isMoving", success);
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }

                if (moveInput.x > 0) 
                { 
                    spriteRenderer.flipX = true;
                    UpdateColliderMirror(false);
                }
                else if (moveInput.x < 0) 
                { 
                    spriteRenderer.flipX = false;
                    UpdateColliderMirror(true);
                }
            }
        }

        private void UpdateColliderMirror(bool faceLeft)
        {
            if (facingLeft == faceLeft) return;

            facingLeft = faceLeft;

            for (int i = 0; i < playerCollider.pathCount; i++)
            {
                Vector2[] mirroredPath = new Vector2[originalPaths[i].Length];
                for (int j = 0; j < originalPaths[i].Length; j++)
                {
                    mirroredPath[j] = originalPaths[i][j];
                    if (!faceLeft)
                        mirroredPath[j].x *= -1;
                }
                playerCollider.SetPath(i, mirroredPath);
            }
        }

        private bool TryMove(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                int count = rb.Cast(direction, movementFilter, castCollisions, speed * Time.fixedDeltaTime + collisionOffset);

                if (count == 0)
                {
                    rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeExp(int exp)
        {
            currentExp += exp;
            while (currentExp >= expToLevelUp)
            {
                currentExp = currentExp - expToLevelUp;
                LevelUp();
            }
            experienceBar.SetExp(currentExp, expToLevelUp);
        }

        public void LevelUp()
        {
            currentLevel++;

            expToLevelUp = (int)(expToLevelUp * gameSettingsSO.LevelExperienceMultiplier);

            levelText.text = $"Lvl: {currentLevel}";
            ShowUpgradesAfterLevelUp(playerWeapons.Weapons.Select(x => x.WeaponInstance).ToList());
        }

        public void ShowUpgradesAfterLevelUp(List<WeaponInstance> weapons)
        {
            var offered = new List<GeneratedUpgrade>();

            for (int i = 0; i < upgradesToShow; i++)
            {
                var weapon = weapons[UnityEngine.Random.Range(0, weapons.Count)];
                offered.Add(generator.GenerateUpgrade(weapon));
            }

            LockMove();
            Time.timeScale = 0f;

            UpgradePanel.Instance.Show(offered, OnUpgradeSelected);
        }

        private void OnUpgradeSelected(GeneratedUpgrade upgrade)
        {
            var weapon = playerWeapons.Weapons.Find(x => x.WeaponInstance.weaponName == upgrade.definition.targetWeaponName);

            weapon.WeaponInstance.ApplyUpgrade(upgrade);

            UnlockMove();
            Time.timeScale = 1f;
        }

        private void Die()
        {
            GameManager.Instance.EndOfGame();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var experience = collision.gameObject.GetComponent<Experience>();

            if (experience != null)
            {
                int receivingExp;
                experience.PickUpExp(out receivingExp);

                TakeExp(receivingExp);
            }
        }
    }
}
