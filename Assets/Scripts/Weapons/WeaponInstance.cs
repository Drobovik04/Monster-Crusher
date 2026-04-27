using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class WeaponInstance
    {
        public WeaponSettingsSO config;

        public string weaponName; // название для поиска логики

        public GameObject weaponPrefab; // если будет нужен префаб на котором все висит, но playerWeapon создает 
        public float offsetRellativeToParent; // насколько далеко надо отодвинуть точку спавна от игрока

        public float damage; // урон
        public float attackSpeed; // кол-во атак в секунду
        public int targets; // кол-во целей
        public float range; // дальность
        public float speed; // скорость движения
        public float size; // размер префаба
        public float lifeSpan; // время жизни для снаряда
        public float critChance; // шанс крита
        public float critMultiplier; // множитель
        public float knockback; // отдача
        public WeaponType weaponType;
        public GameObject projectilePrefab;

        public WeaponInstance(WeaponSettingsSO settings)
        {
            config = settings;
            weaponName = settings.weaponName;
            weaponPrefab = settings.weaponPrefab;
            offsetRellativeToParent = settings.offsetRellativeToParent;
            damage = settings.baseDamage;
            attackSpeed = settings.baseAttackSpeed;
            targets = settings.baseTargets;
            range = settings.baseRange;
            speed = settings.baseSpeed;
            size = settings.size;
            lifeSpan = settings.baseLifeSpan;
            critChance = settings.baseCritChance;
            critMultiplier = settings.baseCritMultiplier;
            knockback = settings.baseKnockback;
            weaponType = settings.weaponType;
            projectilePrefab = settings.weaponProjectilePrefab;
        }

        public void ApplyUpgrade(GeneratedUpgrade up)
        {
            switch (up.definition.type)
            {
                case UpgradeType.Damage:
                    damage += up.rolledValue;
                    break;

                case UpgradeType.AttackSpeed:
                    attackSpeed += up.rolledValue;
                    break;

                case UpgradeType.Range:
                    range += up.rolledValue;
                    break;

                case UpgradeType.Targets:
                    targets += Mathf.RoundToInt(up.rolledValue);
                    break;

                case UpgradeType.ProjectileSpeed:
                    speed += up.rolledValue;
                    break;

                case UpgradeType.LifeSpan:
                    lifeSpan += up.rolledValue;
                    break;
                case UpgradeType.Size:
                    size += up.rolledValue;
                    break;
                case UpgradeType.CritChance:
                    critChance += up.rolledValue;
                    break;
                case UpgradeType.CritMultiplier:
                    critMultiplier += up.rolledValue;
                    break;
                case UpgradeType.Knockback:
                    knockback += up.rolledValue;
                    break;
            }
        }

        public float GetCurrentStatValue(UpgradeType statType)
        {
            return statType switch
            {
                UpgradeType.Damage => damage,
                UpgradeType.AttackSpeed => attackSpeed,
                UpgradeType.Range => range,
                UpgradeType.Targets => targets,
                UpgradeType.ProjectileSpeed => speed,
                UpgradeType.LifeSpan => lifeSpan,
                UpgradeType.Size => size,
                UpgradeType.CritChance => critChance,
                UpgradeType.CritMultiplier => critMultiplier,
                UpgradeType.Knockback => knockback,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
