using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class PlayerWeapons : MonoBehaviour
    {
        [SerializeField] private List<WeaponBehaviour> weapons = new();
        [SerializeField] private GameObject playerGameObject;

        public List<WeaponBehaviour> Weapons => weapons;

        public void AddWeapon(WeaponSettingsSO newWeaponSO)
        {
            var instance = Instantiate(newWeaponSO.weaponPrefab, Vector3.zero, Quaternion.identity, playerGameObject.transform);            
            var behaviour = instance.GetComponent<WeaponBehaviour>();
            behaviour.Init(new WeaponInstance(newWeaponSO));
            weapons.Add(behaviour);
        }
    }

}
