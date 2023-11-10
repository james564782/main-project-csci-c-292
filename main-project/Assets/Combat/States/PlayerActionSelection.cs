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
    int selected = 0;

    public override void StateStart() {
        if (!stateMachine.GetCharacterEntity().GetAlive()) { //If the character is dead then change to the other one and reset action selection
            stateMachine.ChangeCharacter();
            ChangeState("PlayerActionSelection");
        }
        selectionUI.SetActive(true);
    }

    public override void StateUpdate() {
        if (Input.GetKey("d") || Input.GetKey("right")) { //Skills
            skillsText.text = "<b>Skills</b>";
            itemsText.text = "Items";
            tacticsText.text = "Tactics";
            switchText.text = "Switch";
            selected = 0;
        }
        else if (Input.GetKey("w") || Input.GetKey("up")) { //Items
            skillsText.text = "Skills";
            itemsText.text = "<b>Items</b>";
            tacticsText.text = "Tactics";
            switchText.text = "Switch";
            selected = 1;
        }
        else if (Input.GetKey("a") || Input.GetKey("left")) { //Tactics
            skillsText.text = "Skills";
            itemsText.text = "Items";
            tacticsText.text = "<b>Tactics</b>";
            switchText.text = "Switch";
            selected = 2;
        }
        else if (Input.GetKey("s") || Input.GetKey("down")) { //Switch
            skillsText.text = "Skills";
            itemsText.text = "Items";
            tacticsText.text = "Tactics";
            switchText.text = "<b>Switch</b>";
            selected = 3;
        }
        if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
            switch (selected) {
                case 0:
                    stateMachine.PlaySelectionUISound();
                    ChangeState("PlayerSkillSelection");
                    break;
                case 1:
                    stateMachine.PlaySelectionUISound();
                    ChangeState("PlayerItemSelection");
                    break;
                case 2:
                    stateMachine.PlaySelectionUISound();
                    ChangeState("PlayerTacticSelection");
                    break;
                case 3:
                    stateMachine.PlaySelectionUISound();
                    ChangeState("PlayerSwitch");
                    break;
            }
        }

            /*else {
                skillsText.text = "Skills";
                itemsText.text = "Items";
                tacticsText.text = "Tactics";
                switchText.text = "Switch";
            }*/
        }

    public override void StateFixedUpdate() {

    }

    protected override void ExitState() {
        selected = 0;
        skillsText.text = "Skills";
        itemsText.text = "Items";
        tacticsText.text = "Tactics";
        switchText.text = "Switch";
        selectionUI.SetActive(false);
    }

    private void HighlightOption() {

    }
}
