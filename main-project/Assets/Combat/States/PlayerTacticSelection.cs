using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTacticSelection : CombatState {

    public override void StateStart() {

    }

    public override void StateUpdate() {
        if (Input.GetKey("x") || Input.GetKey(KeyCode.Backspace)) { //Back
            ChangeState("PlayerActionSelection");
        }
    }

    public override void StateFixedUpdate() {

    }
}
