using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    CombatStateMachine stateMachine;
    static CombatSystem system;

    [Header("Entity Statistics")]
    [SerializeField] CharacterData[] characterData = new CharacterData[2];

    [Header("Scene Information")]
    [SerializeField] Transform[] playerPosition;
    [SerializeField] Transform[] enemyPosition;

    private void Awake() {
        system = this;
        stateMachine = GetComponent<CombatStateMachine>();
    }


    struct Cell {
        //private Entity entity; //null if nothing
        //private Transform position;
    }


}
