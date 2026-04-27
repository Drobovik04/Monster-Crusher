using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/MonsterSettings")]
    public class MonsterSettingsSO : ScriptableObject
    {
        public float Speed = 1.5f;
        public float Damage = 5f;
        public float DamageInterval = 0.5f;
        public float DistanceToStop = 0;
    }
}
