using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState : MonoBehaviour {

    protected CombatStateMachine stateMachine;
    public virtual void StateStart() {

    }

    public virtual void StateUpdate() {

    }

    public virtual void StateFixedUpdate() { //Move objects on screen during this frame

    }

    public void SetCombatStateMachine(CombatStateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    protected void ChangeState(string stateType) { //Communicates with CombatStateMachine.ChangeState();
        ExitState();
        stateMachine.ChangeState(stateType);
    }
    protected void ChangeState(string stateType, Skill skill) { //Communicates with CombatStateMachine.ChangeState();
        ExitState();
        stateMachine.ChangeState(stateType, skill);
    }

    protected void ChangeState(string stateType, bool endPlayerTurn) {
        ExitState();
        if (endPlayerTurn) {
            stateMachine.EndPlayerTurn();
        }
        else {
            stateMachine.ChangeState(stateType);
        }
    }

    protected virtual void ExitState() {

    }

}
