using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerSkillUsage : CombatState {

    private Skill skill;
    private SkillEvent currentEvent;
    private bool[] currentTarget = new bool[] { false, false, false, false, false, false };
    public override void StateStart() {
        StartCoroutine(SkillUsage());
    }

    public override void StateUpdate() {

    }

    public override void StateFixedUpdate() {

    }
    public void SetSkill(Skill skill) {
        this.skill = skill;
    }

    IEnumerator SkillUsage() {
        Debug.Log("Initial Pause");
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Start Skill Usage");
        foreach (var skillEvent in skill.GetEvents()) {
            if (skillEvent is SkillHealthModification) {
                ResolveHealthModify((SkillHealthModification)skillEvent, currentTarget);
            }
            else if (skillEvent is SkillBleed) {
                Debug.Log("Bleed");
            }
            else if (skillEvent is SkillStatModifier) {
                Debug.Log("Stat Modify");
            }
            else if (skillEvent is SkillTarget) {
                if (((SkillTarget)skillEvent).targets.Length <= 0) {
                    break;
                }
                int enemyCount = CombatSystem.system.GetEnemyCount();
                int selected = 0;
                SkillTarget.Targets[] targets = ((SkillTarget)skillEvent).targets;
                bool[] target = new bool[6];
                if (targets.Length > 1) {
                    target = targets[selected].GetTargets();
                }
                if (targets[selected].GetTargets()[4] || targets[selected].GetTargets()[5]) {
                    enemyCount = 2; //This might not work anymore if one of the players die.
                }
                while (!Input.GetKeyDown("z") && !Input.GetKeyDown(KeyCode.Space) && targets.Length > 1) {
                    if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("a")) {
                        selected = (int)Mathf.Repeat(selected - 1, enemyCount);
                    }
                    else if (Input.GetKeyDown("w") || Input.GetKeyDown("up") || Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
                        selected = (int)Mathf.Repeat(selected + 1, enemyCount);
                    }
                    Highlight(targets[selected].GetTargets());

                    yield return null;
                }
                target = targets[selected].GetTargets();
                currentTarget = target;
                RemoveHighlight();
            }
        }
        stateMachine.EndPlayerTurn();
    }

    private void ResolveHealthModify(SkillHealthModification hpMod, bool[] currentTarget) {
        CombatSystem.Entity[] entity = CombatSystem.system.GetEnemyEntities();
        CombatSystem.Entity[] characters = new CombatSystem.Entity[] { CombatSystem.system.GetCharacterEntity(0), CombatSystem.system.GetCharacterEntity(1) };
        for (int i = 0; i < 4; i++) {
            if (entity[i].GetAlive() && currentTarget[i]) {
                CombatSystem.system.ModifyEnemyHealth(i, -hpMod.GetValue());
            }
        }
        for (int i = 0; i < 2; i++) {
            if (characters[i].GetAlive() && currentTarget[4 + i]) {
                CombatSystem.system.ModifyCharacterHealth(i, -hpMod.GetValue());
            }
        }
    }

    private void Highlight(bool[] target) {
        CombatSystem system = CombatSystem.system;
        for (int i = 0; i < 4; i++) {
            CombatSystem.Entity[] entity = system.GetEnemyEntities();
            if (entity[i].GetAlive()) {
                if (target[i]) {
                    entity[i].ToggleSelected(true);
                    CombatSystem.system.SetEnemyImageSelected(i, true);
                }
                else {
                    entity[i].ToggleSelected(false);
                    CombatSystem.system.SetEnemyImageSelected(i, false);
                }
            }
        }
        CombatSystem.Entity[] characters = new CombatSystem.Entity[] { system.GetCharacterEntity(0), system.GetCharacterEntity(1) };
        if (characters[0].GetAlive()) {
            if (target[4]) {
                characters[0].ToggleSelected(true);
                CombatSystem.system.SetCharacterImageSelected(0, true);
            }
            else {
                characters[0].ToggleSelected(false);
                CombatSystem.system.SetCharacterImageSelected(0, false);
            }
        }
        if (characters[1].GetAlive()) {
            if (target[5]) {
                characters[1].ToggleSelected(true);
                CombatSystem.system.SetCharacterImageSelected(1, true);
            }
            else {
                characters[1].ToggleSelected(false);
                CombatSystem.system.SetCharacterImageSelected(1, false);
            }
        }
    }
    private void RemoveHighlight() {
        CombatSystem.Entity[] entity = CombatSystem.system.GetEnemyEntities();
        for (int i = 0; i < 4; i++) {
            entity[i].ToggleSelected(false);
            CombatSystem.system.SetEnemyImageSelected(i, false);
        }
        CombatSystem.system.GetCharacterEntity(0).ToggleSelected(false);
        CombatSystem.system.GetCharacterEntity(1).ToggleSelected(false);
        CombatSystem.system.SetCharacterImageSelected(0, false);
        CombatSystem.system.SetCharacterImageSelected(1, false);
    }
}