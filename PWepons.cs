using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New weapon", menuName = "Weapon/Common weapon")]

public class PWepons : ScriptableObject
{
    public enum Weapon
    {
        stick, sword, dagger
    }

    [Space(5)]
    [Header("Weapon settins")]
    public Weapon type;
    public new string name;
    public GameObject palyerPrefab;

    [Space(5)]
    public BaseStats.Base baseStats;
}

public class BaseStats : ScriptableObject
{
    [System.Serializable]
    public class Base
    {
        public float damige, range, attackSpeed, spawnRate, speedMultiplyer, knockBack, damigeEffect;
    }
}