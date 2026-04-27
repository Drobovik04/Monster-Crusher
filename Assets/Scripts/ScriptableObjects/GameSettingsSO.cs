using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
    public class GameSettingsSO : ScriptableObject
    {
        public float BasicHealth = 100f;
        public float BasicMoveSpeed = 1f;
        public float BasicMonstersMoveSpeedMultiplier = 1f;
        public int BasicNeededExpForFirstLevel = 100;
        public int BasicDropingExpereinceFromMonster = 10;
        public float LevelExperienceMultiplier = 1.1f;
        public float ExperienceEnemiesMultiplier = 1.065f;
        public float HealthEnemiesMultiplier = 1.2f;
        public float MonsterMoveSpeedMultiplier = 1.08f;
        public float TimeToUpDifficulty = 60f; // seconds
    }
}
