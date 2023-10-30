using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBleedData", menuName = "Skills/Bleed Data")]
public class SkillBleed : SkillEvent {

    [SerializeField] bool initialDamage;
    [SerializeField][Range(-255, 255)] int damage;
    [SerializeField][Range(0, 32)] int numberOfTurns;

}
