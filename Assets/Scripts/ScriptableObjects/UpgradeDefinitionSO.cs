using Assets.Scripts.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Upgrades/Upgrade Definition")]
    public class UpgradeDefinitionSO : ScriptableObject
    {
        public string title;
        public string description;
        public Sprite icon;

        public UpgradeRarity rarity;
        public UpgradeType type;

        public string targetWeaponName;

        public float minValue;
        public float maxValue;
    }
}
