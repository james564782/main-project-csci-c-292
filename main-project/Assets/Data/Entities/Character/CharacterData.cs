using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data")]
public class CharacterData : EntityData
{
    //List<Skill> skillList = new List<Skill>();
    //Skill[] equippedSkill = new Skill[4];
    [Header("Skills")]
    [SerializeField] List<Skill> skills;
    [SerializeField] Skill[] equippedSkills;

    [Header("Info")]
    [SerializeField] Sprite defaultSprite;


    //Getter Methods
    public List<Skill> GetAllSkills() { return skills; }
    public Skill[] GetEquippedSkills() { return equippedSkills; }
    public Skill GetEquippedSkill(int index) {  return equippedSkills[index]; }
    public Sprite GetSprite() { return defaultSprite; }

}
