using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEvent : ScriptableObject
{
    //[SerializeField] protected SkillEvent dependent; //Won't be doing this since it would be difficult with ScriptableObjects
    [SerializeField] protected bool qTE; //Save the quick time event into this slot, if it is null then there is no event. //Will get a float value from the QTE from 0 to 1.
    [SerializeField] protected Animation animation; //Will save a specific animation into the slot. If it is null then it will just apply the event as normal.
}
