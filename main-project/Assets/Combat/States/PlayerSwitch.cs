using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : CombatState {

    public override void StateStart() {
        stateMachine.ChangeCharacter();
        stateMachine.ChangeState("PlayerActionSelection");
    }

    public override void StateUpdate() {

    }

    public override void StateFixedUpdate() {

    }
}
