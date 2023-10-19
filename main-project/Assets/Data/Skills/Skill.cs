using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Skill Data")]
public class Skill : ScriptableObject
{
    [SerializeField] SkillEvent[] events;
}