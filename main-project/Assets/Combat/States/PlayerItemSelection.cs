using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemSelection : CombatState {

    //int itemSelection = 0;
    public override void StateStart() {

    }

    public override void StateUpdate() {
        if (Input.GetKey("x") || Input.GetKey(KeyCode.Backspace)) { //Back
            ChangeState("PlayerActionSelection");
        }
    }

    public override void StateFixedUpdate() {

    }
    private void HasItems() {

    }
}
