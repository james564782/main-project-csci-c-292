using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTargetData", menuName = "Skills/Target Data")]
public class SkillTarget : SkillEvent {

    public Targets[] targets;

    [System.Serializable]
    public struct Targets {
        [SerializeField] public bool[] target;
        public bool[] GetTargets() {
            return target;
        }
        public int GetTargetCount() {
            int i = 0;
            foreach (bool t in target) {
                if (t) {
                    i++;
                }
            }
            return i;
        }
        public bool GetTarget(int index) {
            return target[index];
        }
    }

    //[enemy zero] [enemy one] [enemy two] [enemy three] [ally] [self]

    //If a skill has the target event, but targets is null or has a size of 0. Then it uses the same target as the previous target event in the same skill.
    //If it doesn't have a target event, then it is an error
    //If the target is dead, then the attack ends
}
