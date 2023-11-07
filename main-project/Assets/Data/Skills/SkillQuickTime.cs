using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuickTimeData", menuName = "Skills/Quick-Time Data")]
public class SkillQuickTime : SkillEvent {

    [SerializeField][Range(0, 1)] float point; //on a scale of 0 to 1.
    [SerializeField][Range(0, 1)] float size; //on a scale of 0 to 1.
    [SerializeField][Range(0, 3)] float rate; //On a scale of 0 to 3.
    public float GetPoint() {
        return point;
    }
    public float GetSize() { 
        return size; 
    }
    public float GetMin() {
        return point - size;
    }
    public float GetMax() {
        return point + size;
    }
    public float GetRate() {
        return rate;
    }

}