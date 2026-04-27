using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Upgrades
{
    public class UpgradeGenerator : MonoBehaviour
    {
        [SerializeField] private PlayerWeapons playerWeapons;

        public UpgradeRarityTableSO rarityTable;
        public List<UpgradeDefinitionSO> allUpgrades;

        public void Start()
        {
            allUpgrades.Clear();
            Addressables.LoadAssetsAsync<UpgradeDefinitionSO>("UpgradePool", (upgrade) =>
            {
                allUpgrades.Add(upgrade);
            });
        }

        // генерация полня хрень, я так думаю, пока оставить для тестирования, но потом надо подумать как ее переделать ( в палне GenRandomRarity, надо подумать как сделать)
        public GeneratedUpgrade GenerateUpgrade(WeaponInstance weapon)
        {
            UpgradeRarity rarity = rarityTable.GetRandomRarity();

            var candidates = allUpgrades
                .Where(u => u.rarity == rarity && u.targetWeaponName == weapon.weaponName)
                .ToList();

            if (candidates.Count == 0)
            {
                Debug.LogWarning($"There is no upgrades for '{weapon.weaponType}' with '{rarity}' rarity");
                return null;
            }

            var selected = candidates[Random.Range(0, candidates.Count)];

            float value = Random.Range(selected.minValue, selected.maxValue);

            return new GeneratedUpgrade
            {
                definition = selected,
                rolledValue = value,
                currentValue = playerWeapons.Weapons.Find(x => x.WeaponInstance.weaponName == selected.targetWeaponName).WeaponInstance.GetCurrentStatValue(selected.type)
            };
        }
    }
}
