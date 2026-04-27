using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utilities.UI_Score
{
    public class MaxScore : MonoBehaviour
    {
        [SerializeField] TMP_Text maxScoreTMP;
        private void Start()
        {
            maxScoreTMP.text = 
                $"Max score: {DataStorage.Instance.PlayerData.MaximumScore}\n" +
                $"in {DataStorage.Instance.PlayerData.DurationOfRunWithMaxScore:hh\\:mm\\:ss}";
        }
    }
}
