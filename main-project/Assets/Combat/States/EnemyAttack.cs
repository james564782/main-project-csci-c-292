using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : CombatState {

    public override void StateStart() {
        StartCoroutine(TestingEndState());
    }

    public override void StateUpdate() {

    }

    IEnumerator TestingEndState() {
        for (int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(1);
            Debug.Log("Testing Enemy Attack: " + i);
        }
        ChangeState("PlayerActionSelection");
    }

    public override void StateFixedUpdate() {

    }
}
