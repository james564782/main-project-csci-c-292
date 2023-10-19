using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data")]
public class CharacterData : EntityData
{
    //List<Skill> skillList = new List<Skill>();
    //Skill[] equippedSkill = new Skill[4];

    [SerializeField] List<Skill> skills;
    [SerializeField] Skill[] equippedSkills;

}
