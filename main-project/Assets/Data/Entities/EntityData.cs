using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : ScriptableObject {

    [Header("Stats")]
    [SerializeField][Range(1, 100)] protected int max_Vitality; //Health
    [SerializeField][Range(1, 100)] protected int max_SP;
    [SerializeField][Range(1, 100)] protected int power;
    protected int current_Vitality;
    protected int current_SP;
    protected int[,] bleed; //[[turns, damage][turns, damage],[turns, damage]]
    protected float[,] statModification; //[[modifier, count]]
    protected bool alive = true;

    public int GetCurrentVitality() {
        return current_Vitality;
    }
    public int GetMaxVitality() {
        return max_Vitality;
    }
    public int GetCurrentSP() {
        return current_SP;
    }
    public int GetMaxSP() {
        return max_SP;
    }
    public int GetPower() {
        return power;
    }
    public void SetCurrentVitality(int new_Vitality) {
        current_Vitality = Mathf.Clamp(new_Vitality, 0, max_Vitality);
        if (current_Vitality <= 0) {
            alive = false;
        }
    }
    public void ModifyCurrentVitality(int modified_Vitality) {
        current_Vitality = Mathf.Clamp(current_Vitality + modified_Vitality, 0, max_Vitality);
        if (current_Vitality <= 0) {
            alive = false;
        }
    }
    public void SetCurrentSP(int new_SP) {
        current_SP = Mathf.Clamp(new_SP, 0, max_SP);
    }
    public void ModifyCurrentSP(int modified_SP) {
        current_SP = Mathf.Clamp(current_SP + modified_SP, 0, max_SP);
    }
}
