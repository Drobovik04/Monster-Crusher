using Assets.Scripts.Monster;
using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private GameObject expPrefab;
    private DamagePopupSpawner popupSpawner;
    private int expDropingQuantity;

    private void Awake()
    {
        popupSpawner = DamagePopupSpawner.Instance;
    }

    public float Health
    {
        get
        {
            return health; 
        }
        set
        {
            TakeDamage(value);
        }
    }

    public void TakeDamage(float damage, bool isHitCrit = false)
    {
        health -= damage;
        popupSpawner.ShowDamage(damage, transform.position + Vector3.up * 0.15f, isHitCrit ? new Color(255, 165, 0) : Color.red);

        if (health <= 0)
        {
            Die();
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetExpDrop(int exp)
    {
        expDropingQuantity = exp;
    }

    private void DropExp(int expForShard)
    {
        var expObj = Instantiate(expPrefab, transform.position, Quaternion.identity);
        var expScript = expObj.GetComponent<Experience>();
        expScript.SetExp(expForShard);
    }

    public void Die()
    {
        DropExp(expDropingQuantity);

        MonstersManager.Instance.IncreaseKillCounterBy(1);
        MonstersManager.Instance.RemoveMonster(gameObject);
        Destroy(gameObject);
    }
}
