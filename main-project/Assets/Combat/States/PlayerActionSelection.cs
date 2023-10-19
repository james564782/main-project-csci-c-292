using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerActionSelection : CombatState
{
    [SerializeField] GameObject selectionUI;
    [SerializeField] TextMeshProUGUI skillsText;
    [SerializeField] TextMeshProUGUI itemsText;
    [SerializeField] TextMeshProUGUI tacticsText;
    [SerializeField] TextMeshProUGUI switchText;

    public override void StateStart() {
        selectionUI.SetActive(true);
    }

    public override void StateUpdate() {
        if (Input.GetKey("d") || Input.GetKey("right")) { //Skills
            skillsText.text = "<b>Skills</b>";
            itemsText.text = "Items";
            tacticsText.text = "Tactics";
            switchText.text = "Switch";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                ChangeState("PlayerSkillSelection");
            }
        }
        else if (Input.GetKey("w") || Input.GetKey("up")) { //Items
            skillsText.text = "Skills";
            itemsText.text = "<b>Items</b>";
            tacticsText.text = "Tactics";
            switchText.text = "Switch";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                ChangeState("PlayerItemSelection");
            }
        }
        else if (Input.GetKey("a") || Input.GetKey("left")) { //Tactics
            skillsText.text = "Skills";
            itemsText.text = "Items";
            tacticsText.text = "<b>Tactics</b>";
            switchText.text = "Switch";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                ChangeState("PlayerTacticSelection");
            }
        }
        else if (Input.GetKey("s") || Input.GetKey("down")) { //Switch
            skillsText.text = "Skills";
            itemsText.text = "Items";
            tacticsText.text = "Tactics";
            switchText.text = "<b>Switch</b>";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                ChangeState("PlayerSwitch");
            }
        }
        else {
            skillsText.text = "Skills";
            itemsText.text = "Items";
            tacticsText.text = "Tactics";
            switchText.text = "Switch";
        }
    }

    public override void StateFixedUpdate() {

    }

    protected override void ExitState() {
        selectionUI.SetActive(false);
    }

    private void HighlightOption() {

    }
}
