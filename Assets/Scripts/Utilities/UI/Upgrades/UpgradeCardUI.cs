using Assets.Scripts.Upgrades;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Utilities.UI.Upgrades
{
    public class UpgradeCardUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text upgradeValue;
        [SerializeField] private Image icon;
        [SerializeField] private Image rarityBorder;
        [SerializeField] private List<UpgradeRarityColorPair> rarityColor;

        private Action onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        public void SetData(GeneratedUpgrade up, Action onClick)
        {
            title.text = up.definition.title;
            description.text = up.definition.description;
            if (up.definition.type == UpgradeType.CritChance)
            {
                upgradeValue.text = $"From {up.currentValue:0.00}% to {(up.currentValue + up.rolledValue):0.00}%";
            }
            else
            {
                upgradeValue.text = $"From {up.currentValue:0.00} to {(up.currentValue + up.rolledValue):0.00}";
            }
            icon.sprite = up.definition.icon;

            rarityBorder.color = GetRarityColor(up.definition.rarity);

            this.onClick = onClick;
        }

        private Color GetRarityColor(UpgradeRarity rarity)
        {
            return rarityColor.Find(x => x.Rarity == rarity).Color;
        }
    }
}
