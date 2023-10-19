using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : ScriptableObject {

    [Header("Stats")]
    [SerializeField][Range(1, 100)] protected int max_Vitality;
    [SerializeField][Range(1, 100)] protected int max_Instinct;
    [SerializeField][Range(1, 100)] protected int max_Power;
    protected int current_Vitality;
    protected int current_Instinct;
    protected int current_Power;


}
