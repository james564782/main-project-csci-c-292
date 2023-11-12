using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttack : CombatState {

    [SerializeField] GameObject enemyAttackUI;
    private bool[] coroutineRunning = new bool[] { false, false, false, false }; //only change to true if a coroutine is running
    private bool[] invulnerable = new bool[] { false, false };
    public override void StateStart() {
        enemyAttackUI.SetActive(true);
        CombatSystem.system.ResolveAllEnemyBleeding();
        if (!stateMachine.GetEnded()) {
            StartCoroutine(AttackPhase());
        }
    }

    public override void StateUpdate() {
        if (Input.GetKeyDown("z") && !invulnerable[0] && CombatSystem.system.GetCharacterEntity(0).GetAlive()) { //Player 1 Defense
            StartCoroutine(Jump(0));
        }
        if (Input.GetKeyDown("x") && !invulnerable[1] && CombatSystem.system.GetCharacterEntity(1).GetAlive()) { //Player 2 Defense
            StartCoroutine(Jump(1));
        }
    }

    IEnumerator AttackPhase() {
        CombatSystem.Entity[] enemies = CombatSystem.system.GetEnemyEntities();
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i].GetAlive()) {
                StartCoroutine(Attack(enemies[i], i, GetRandomLivingPlayer()));
                yield return new WaitForSeconds(Random.Range(0.75f, 2.00f));
            }
        }
        while (coroutineRunning[0] || coroutineRunning[1] || coroutineRunning[2] || coroutineRunning[3]) {
            yield return null;
        }
        ChangeState("PlayerActionSelection");
    }

    private int GetRandomLivingPlayer() {
        int random = Random.Range(0, 2);
        if (CombatSystem.system.GetCharacterEntity(random).GetAlive()) {
            return random;
        }
        else {
            return (random < 1 ? 1 : 0);
        }
    }

    IEnumerator Jump(int index) {
        invulnerable[index] = true;
        GameObject character = CombatSystem.system.GetCharacterGameObject(index == 0 ? true : false);
        character.GetComponent<SpriteRenderer>().color = Color.cyan;
        float startY = character.transform.position.y;
        float jumpHeight = 0.75f;
        for (float i = 0; i < 1; i += Time.deltaTime) {
            float value = (-Mathf.Pow(i - 0.5f, 2) * 4 + 1);
            character.transform.position = new Vector3(character.transform.position.x, startY + value * jumpHeight, 0);
            yield return null;
        }
        invulnerable[index] = false;
        character.GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator Attack(CombatSystem.Entity enemy, int index, int target) {
        coroutineRunning[index] = true;
        bool attacked = false;
        float attackRate = Random.Range(4.5f, 6.0f);
        float returnRate = 0.6f;
        GameObject obj = enemy.gameObject;
        Vector3 startPosition = obj.transform.position;
        Vector3 direction = (CombatSystem.system.GetCharacterEntity(target).gameObject.transform.position - obj.transform.position).normalized;
        float time = 0;
        float jumpHeight = 0.3f;
        float jumpRate = 2.65f;
        Vector3 actualPosition = obj.transform.position;
        while (obj.transform.position.x > -10) {
            time += Time.deltaTime * jumpRate;
            float value = (-Mathf.Pow(Mathf.Repeat(time, 1) - 0.5f, 2) * 4 + 1);
            Vector3 up = Vector3.up * value * jumpHeight;
            actualPosition += direction * attackRate * Time.deltaTime;
            obj.transform.position = actualPosition + up;
            obj.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(obj.transform.position.y);
            if (Mathf.Abs(obj.transform.position.x - CombatSystem.system.GetCharacterEntity(target).gameObject.transform.position.x) < 0.4 && !attacked && !invulnerable[target]) {
                attacked = true;
                CombatSystem.system.ModifyCharacterHealth(target, -enemy.data.GetPower());
            }
            yield return null;
        }
        obj.transform.position = startPosition + Vector3.right * 9f;
        for (float i = 0; i < 1; i += Time.deltaTime * returnRate) {
            float value = (-Mathf.Pow(Mathf.Repeat(i * jumpRate * 1.5f, 1) - 0.5f, 2) * 4 + 1);
            Vector3 lerp = Vector3.Lerp(startPosition + Vector3.right * 10f, startPosition, i);
            obj.transform.position = new Vector3(lerp.x, lerp.y + value * jumpHeight, 0);
            obj.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(obj.transform.position.y);
            yield return null;
        }
        obj.transform.position = startPosition;
        coroutineRunning[index] = false;
    }

    public override void StateFixedUpdate() {

    }
    protected override void ExitState() {
        enemyAttackUI.SetActive(false);
        stateMachine.HighlightCharacter(true);
    }
}
