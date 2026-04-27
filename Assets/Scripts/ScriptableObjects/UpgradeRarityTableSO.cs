using Assets.Scripts.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Upgrades/Rarity Table")]
    public class UpgradeRarityTableSO : ScriptableObject
    {
        public WeightedValue<UpgradeRarity>[] rarities;

        public UpgradeRarity GetRandomRarity()
        {
            float total = rarities.Sum(r => r.weight);
            float roll = UnityEngine.Random.value * total;

            foreach (var r in rarities)
            {
                roll -= r.weight;
                if (roll <= 0)
                    return r.value;
            }

            return rarities[rarities.Length - 1].value;
        }
    }
}
