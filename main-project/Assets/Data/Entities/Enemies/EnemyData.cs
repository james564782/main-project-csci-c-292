using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data")]
public class EnemyData : EntityData {

    [Header("Info")]
    [SerializeField] string name;
    [SerializeField] Sprite defaultSprite;


    public Sprite GetSprite() { return defaultSprite; }
    public string GetName() { return name; }
}
