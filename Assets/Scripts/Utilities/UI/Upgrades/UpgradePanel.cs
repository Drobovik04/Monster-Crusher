using Assets.Scripts.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utilities.UI.Upgrades
{
    public class UpgradePanel : MonoBehaviour
    {
        public static UpgradePanel Instance;

        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardsParent;
        [SerializeField] private CanvasGroup canvasGroup;

        private Action<GeneratedUpgrade> onSelect;

        private void Awake()
        {
            Instance = this;
            Hide();
        }

        public void Show(List<GeneratedUpgrade> upgrades, Action<GeneratedUpgrade> selectCallback)
        {
            ClearCards();
            onSelect = selectCallback;

            foreach (var up in upgrades)
            {
                var cardObj = Instantiate(cardPrefab, cardsParent);
                var card = cardObj.GetComponent<UpgradeCardUI>();
                card.SetData(up, () => SelectUpgrade(up));
            }

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            ClearCards();
        }

        private void ClearCards()
        {
            for (int i = cardsParent.childCount - 1; i >= 0; i--)
            {
                Destroy(cardsParent.GetChild(i).gameObject);
            }
        }

        private void SelectUpgrade(GeneratedUpgrade up)
        {
            onSelect?.Invoke(up);
            Hide();
        }
    }
}
