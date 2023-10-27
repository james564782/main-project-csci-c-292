using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetSelection : CombatState {

    private int selected = 0; //Left is 0, Top is 1, Bottom is 2, Right is 3
    private int enemyCount = 0;
    public override void StateStart() {
        enemyCount = CombatSystem.system.GetEnemyCount();
        if (enemyCount <= 0) {
            Debug.Log("Something is wrong in PlayerTargetSelection, enemy count is " + enemyCount);
        }
    }

    public override void StateUpdate() {
        if (Input.GetKey("x") || Input.GetKey(KeyCode.Backspace)) {
            ChangeState("PlayerSkillSelection");
        }

        if ((Input.GetKeyDown("a") || Input.GetKeyDown("left")) && enemyCount <= 1) { //Left
            selected = 0;
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {

            }
        }
        else if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")) && enemyCount <= 2) { //Top
            selected = 1;
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {

            }
        }
        else if ((Input.GetKeyDown("s") || Input.GetKeyDown("down")) && enemyCount <= 3) { //Bottom
            selected = 2;
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {

            }
        }
        else if ((Input.GetKeyDown("d") || Input.GetKeyDown("right")) && enemyCount <= 4) { //Right
            selected = 4;
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {

            }
        }
    }

    public override void StateFixedUpdate() {

    }
}