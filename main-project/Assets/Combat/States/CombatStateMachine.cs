using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : MonoBehaviour
{
    CombatState[] states;
    CombatState currentState;
    bool selectedCharacter = true; //true for dale, false for gail
    int playerTurns = 2;

    private void Awake() {
        InitializeVariables();
    }

    private void InitializeVariables() {
        states = GetComponents<CombatState>();
        foreach (CombatState state in states) {
            state.SetCombatStateMachine(this);
            if (state is PlayerActionSelection) {
                currentState = state;
            }
        }
    }

    public bool GetCharacter() { //true for dale, false for gail
        return selectedCharacter;
    }
    public void ChangeCharacter(bool character) { //true for dale, false for gail
        selectedCharacter = character; 
    }
    public void ChangeCharacter() { //true for dale, false for gail
        selectedCharacter = !selectedCharacter;
    }

    public void ChangeState(string stateType) { //Communicates with CombatState.ChangeState();
        currentState = GetState(stateType);
        Debug.Log("Changed to type: " + currentState.GetType());

        try {
            currentState.StateStart();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }

    private CombatState GetState(string stateType) {
        foreach (CombatState state in states) {
            if (state.GetType() == Type.GetType(stateType)) {
                return state;
            }
        }
        Debug.Log("Can't Get State/State does not exist");
        return currentState;
    }

    private void Start() {
        try {
            currentState.StateStart();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }
    private void Update() {
        try {
            currentState.StateUpdate();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }
    private void FixedUpdate() {
        try {
            currentState.StateFixedUpdate();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }


}