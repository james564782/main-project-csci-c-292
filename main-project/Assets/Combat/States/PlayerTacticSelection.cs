using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTacticSelection : CombatState {

    [SerializeField] GameObject selectionUI;
    [SerializeField] TextMeshProUGUI skipTurnText;
    [SerializeField] TextMeshProUGUI runAwayText;
    private int selected = 0;

    public override void StateStart() {
        selected = 0;
        selectionUI.SetActive(true);
    }

    public override void StateUpdate() {
        if (Input.GetKey("x") || Input.GetKey(KeyCode.Backspace)) { //Back
            ChangeState("PlayerActionSelection");
        }

        if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) { //Change Selection
            selected = selected == 1 ? 0 : 1;
        }
        if (selected == 0) {
            skipTurnText.text = "<b>Skip Turn</b>";
            runAwayText.text = "Run Away";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                ChangeState("PlayerActionSelection", true);
            }
        }
        else if (selected == 1) {
            skipTurnText.text = "Skip Turn";
            runAwayText.text = "<b>Run Away</b>";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                //ChangeState("Run Away");
                Debug.Log("Changed to type: Run Away (Didn't Actually Change Though)");
            }
        }
    }

    protected override void ExitState() {
        selectionUI.SetActive(false);
    }

    public override void StateFixedUpdate() {

    }
}
