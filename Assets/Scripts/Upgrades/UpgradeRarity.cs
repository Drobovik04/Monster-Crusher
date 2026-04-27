using System;

namespace Assets.Scripts.Upgrades
{
    public enum UpgradeRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    [Serializable]
    public struct UpgradeRarityColorPair 
    {
        public UpgradeRarity Rarity;
        public UnityEngine.Color Color;
    }

}
