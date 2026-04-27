using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private int experinceQuantity;

        public int ExperienceQuantity => experinceQuantity;

        public void SetExp(int expQuantity)
        {
            experinceQuantity = expQuantity;
        }

        public void PickUpExp(out int exp)
        {
            exp = experinceQuantity;
            Destroy(gameObject);
        }
    }
}
