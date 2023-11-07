using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class PlayerSkillUsage : CombatState {

    private Skill skill;
    private SkillEvent currentEvent;
    [SerializeField] GameObject quickTimeUI;
    [SerializeField] GameObject quickTimeSliderGameObject;
    [SerializeField] Slider quickTimeSlider;
    [SerializeField] Image quickTimeImage;
    private bool[] currentTarget = new bool[] { false, false, false, false, false, false };
    private float proximity; //0 to 1, the closer the quick time event to the center then the higher the value. Or just 0 to 1. If less than 0 then there is no proximity
    public override void StateStart() {
        proximity = -1;
        StartCoroutine(SkillUsage());
    }

    public override void StateUpdate() {

    }

    public override void StateFixedUpdate() {

    }
    protected override void ExitState() {
        quickTimeUI.SetActive(false);
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
                CombatSystem.system.CharacterAttack(stateMachine.GetCharacter());
            }
            else if (skillEvent is SkillBleed) {
                ResolveBleed((SkillBleed)skillEvent, currentTarget);
                CombatSystem.system.CharacterAttack(stateMachine.GetCharacter());
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
            else if (skillEvent is SkillQuickTime) {
                SkillQuickTime quickTime = (SkillQuickTime)skillEvent;
                if (quickTime.GetRate() <= 0) {
                    proximity = -1; //if the rate is 0 or less then the proximity is reset.
                    break;
                }
                quickTimeUI.SetActive(true);
                CreateSliderBackground(quickTime);
                for (float i = 0; i <= 1; i += Time.deltaTime * quickTime.GetRate()) { //Quick Time
                    yield return null;
                    quickTimeSlider.value = i;
                    if (Input.GetKeyDown("z") || Input.GetKeyDown(KeyCode.Space)) {
                        float v = Mathf.Abs(quickTimeSlider.value - quickTime.GetPoint()); //distance to point
                        if (v < quickTime.GetSize()) {
                            proximity = 1;
                        }
                        else {
                            proximity = 0.5f;
                        }
                        yield return new WaitForSeconds(0.2f);
                        break;
                    }
                    proximity = 0.5f;
                }
                quickTimeUI.SetActive(false);
            }
        }
        ChangeState("PlayerActionSelection", true);
    }

    private void CreateSliderBackground(SkillQuickTime quickTime) {
        int width = 256;
        Texture2D texture = new Texture2D(width, 1, TextureFormat.ARGB32, false);
        for (int i = 0; i < width; i++) {
            if (quickTime.GetMin() * width < i && quickTime.GetMax() * width > i) {
                texture.SetPixel(i, 0, Color.green);
            }
            else {
                texture.SetPixel(i, 0, Color.black);
            }
        }
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        quickTimeImage.sprite = sprite;
    }

    private void ResolveBleed(SkillBleed bleed, bool[] currentTarget) { //Note bleeding does not work on characters as of this moment
        CombatSystem.Entity[] entity = CombatSystem.system.GetEnemyEntities();
        for (int i = 0; i < 4; i++) {
            if (entity[i].GetAlive() && currentTarget[i]) {
                CombatSystem.system.InflictBleedingToEnemy(i, bleed.GetNumberOfTurns(), bleed.GetDamage());
            }
        }
    }

    private void ResolveHealthModify(SkillHealthModification hpMod, bool[] currentTarget) {
        CombatSystem.Entity[] entity = CombatSystem.system.GetEnemyEntities();
        CombatSystem.Entity[] characters = new CombatSystem.Entity[] { CombatSystem.system.GetCharacterEntity(0), CombatSystem.system.GetCharacterEntity(1) };
        int hpModValue = -hpMod.GetValue();
        if (proximity > 0) {
            hpModValue = Mathf.RoundToInt(hpModValue * ((float)proximity));
        }
        for (int i = 0; i < 4; i++) {
            if (entity[i].GetAlive() && currentTarget[i]) {
                CombatSystem.system.ModifyEnemyHealth(i, hpModValue);
            }
        }
        for (int i = 0; i < 2; i++) {
            if (characters[i].GetAlive() && currentTarget[4 + i]) {
                CombatSystem.system.ModifyCharacterHealth(i, hpModValue);
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