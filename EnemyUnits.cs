using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/BasicUnit")]
public class EnemyUnits : ScriptableObject
{
    public enum UnitType
    {
        Knigt,
        Archer,
        Pessant
    }
    [Space(5)]
    [Header("Unit Settings")]
    public UnitType type;
    public new string name;
    public GameObject unitEnemyPrefab;

    [Space(5)]
    public UnitStatType.Base baseStats;
}
public class UnitStatType : ScriptableObject
{
    [System.Serializable]
    public class Base
    {
        public float helth, attack, attackrange, attacspeed, /*armor,*/ aggroRange, speed;

    }
}