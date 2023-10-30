using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatModifierData", menuName = "Skills/Stat Modifier Data")]
public class SkillStatModifier : SkillEvent {

    [SerializeField][Range(0, 4)] float modifier;
    [SerializeField][Range(0, 4)] int count; //The character will keep a number of buffs until they are all used. Can double the effects of both healing and damage

}