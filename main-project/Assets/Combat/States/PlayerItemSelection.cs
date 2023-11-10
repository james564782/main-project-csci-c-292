using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItemSelection : CombatState {

    //int itemSelection = 0;
    [SerializeField] GameObject selectionUI;
    //[SerializeField] TextMeshProUGUI[] itemOptionText;
    public override void StateStart() {
        selectionUI.SetActive(true);
    }

    public override void StateUpdate() {
        if (Input.GetKey("x") || Input.GetKey(KeyCode.Backspace)) { //Back
            stateMachine.PlaySelectionUISound();
            ChangeState("PlayerActionSelection");
        }
        if (Input.GetKeyDown("z") || Input.GetKeyDown(KeyCode.Space)) {
            stateMachine.SpendSP(-7);
            stateMachine.PlaySelectionUISound();
            ChangeState("PlayerActionSelection", true);
        }
    }

    public override void StateFixedUpdate() {

    }
    protected override void ExitState() {
        selectionUI.SetActive(false);
    }
}
