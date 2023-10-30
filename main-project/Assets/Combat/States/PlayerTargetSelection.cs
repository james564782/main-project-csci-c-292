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

    public override void StateFixedUpdate() {

    }
}