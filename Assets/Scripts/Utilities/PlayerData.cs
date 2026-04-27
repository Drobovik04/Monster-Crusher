using NUnit.Framework;
using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    //public float BasicHealth = 100f;
    //public float BasicDamage = 1f;
    //public float BasicMoveSpeed  = 1f;
    //public float BasicAttackSpeed = 1f;
    //public float BasicMonstersMoveSpeed = 1f;
    public int MaximumScore = 0;
    public TimeSpan DurationOfRunWithMaxScore = TimeSpan.Zero;
}
