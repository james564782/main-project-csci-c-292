using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthModificationData", menuName = "Skills/Health Modification Data")]
public class SkillHealthModification : SkillEvent {

    //100 = 1.00 x Offense Stat damage.
    [SerializeField][Range(-255, 255)] int value;

    public int GetValue() {
        return value;
    }

}