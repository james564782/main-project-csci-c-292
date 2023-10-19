using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageData", menuName = "Damage Data")]
public class SkillDamage : SkillEvent {
    
    //100 = 1.00 x Offense Stat damage.
    [SerializeField] [Range(0, 511)] int damage;

}
