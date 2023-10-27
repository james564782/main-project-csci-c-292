using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSkillSelection : CombatState {

    [SerializeField] GameObject selectionUI;
    [SerializeField] TextMeshProUGUI[] skillOptionText;
    [SerializeField] TextMeshProUGUI[] skillSpText;
    private int selected = 0;
    Skill[] skill;

    public override void StateStart() {
        selected = 0;
        selectionUI.SetActive(true);
        skill = stateMachine.GetSkillList();
        for (int i = 0;i < 4; i++) {
            if (i < skill.Length) {
                skillOptionText[i].enabled = true;
                skillSpText[i].enabled = true;
                skillOptionText[i].text = skill[i].name;
                skillSpText[i].text = skill[i].GetSPCost() + " SP";
            }
            else {
                skillOptionText[i].enabled = false;
                skillSpText[i].enabled = false;
            }
        }
    }

    public override void StateUpdate() {
        if (Input.GetKey("x") || Input.GetKey(KeyCode.Backspace)) { //Back
            ChangeState("PlayerActionSelection");
        }
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down")) {
            skillOptionText[selected].text = skill[selected].name;
            skillSpText[selected].text = skill[selected].GetSPCost() + " SP";
            selected = (int)Mathf.Repeat(selected + 1, skill.Length);
        } 
        else if (Input.GetKeyDown("w") || Input.GetKeyDown("up")) {
            skillOptionText[selected].text = skill[selected].name;
            skillSpText[selected].text = skill[selected].GetSPCost() + " SP";
            selected = (int)Mathf.Repeat(selected - 1, skill.Length);
        }
        skillOptionText[selected].text = "<b>" + skill[selected].name + "</b>";
        skillSpText[selected].text = "<b>" + skill[selected].GetSPCost() + " SP" + "</b>";
    }

    protected override void ExitState() {
        selectionUI.SetActive(false);
    }

    public override void StateFixedUpdate() {

    }
}
