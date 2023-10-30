using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Entities/Enemy Data")]
public class EnemyData : EntityData {

    [Header("Info")]
    [SerializeField] string entityName;
    [SerializeField] Sprite defaultSprite;


    public Sprite GetSprite() { return defaultSprite; }
    public string GetName() { return entityName; }
}
