using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    public enum WeaponType { Melee, Range, Magic }

    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Game/WeaponSettings")]
    public class WeaponSettingsSO : ScriptableObject
    {
        public string weaponName;

        [Header("WeaponPrefab")]
        public GameObject weaponPrefab;
        public float offsetRellativeToParent;
        public GameObject weaponProjectilePrefab;

        [Header("Base stats")]
        public float baseDamage;
        public float baseAttackSpeed;
        public int baseTargets;
        public float baseRange;
        public float size;
        public float baseCritChance;
        public float baseCritMultiplier;
        public float baseKnockback;

        [Header("Range attack stats")]
        public float baseSpeed;
        public float baseLifeSpan;

        [Header("Behavior")]
        public WeaponType weaponType;
    }
}
