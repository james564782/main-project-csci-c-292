using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEvent : ScriptableObject
{
    //[SerializeField] protected SkillEvent dependent; //Won't be doing this since it would be difficult with using ScriptableObjects. Just have stuff be dependent in the order they are placed.
    [SerializeField] protected Animation animation; //Will save a specific animation into the slot. If it is null then it will just apply the event as normal.
}
