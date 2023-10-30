using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    CombatStateMachine stateMachine;
    public static CombatSystem system;

    [Header("Entity Statistics")]
    [SerializeField] CharacterData[] characterData = new CharacterData[2];
    [SerializeField] EnemyData[] enemyData = new EnemyData[4];

    [Header("Scene Information")]
    [SerializeField] Transform[] characterPosition;
    [SerializeField] Transform[] enemyPosition;
    [SerializeField] Image[] characterImage;
    [SerializeField] Slider[] characterHealthBar;
    [SerializeField] TextMeshProUGUI[] characterHealthText;
    [SerializeField] Slider[] characterSpBar;
    [SerializeField] TextMeshProUGUI[] characterSpText;
    [SerializeField] Image[] enemyImage;
    [SerializeField] Slider[] enemyHealthBar;
    [SerializeField] TextMeshProUGUI[] enemyName;

    [Header("Prefabs")]
    [SerializeField] GameObject[] characterPrefab;
    [SerializeField] GameObject[] enemyPrefab;



    Cell[] playerCell = new Cell[2];
    Cell[] enemyCell = new Cell[4];
    Entity[] characterEntity;
    Entity[] enemyEntity;


    private int turnNumber = 0; //Each time it is the players turn, increase by 1. Starts at 0.

    private void Awake() { //Use this to set up the game.
        system = this;
        stateMachine = GetComponent<CombatStateMachine>();
        turnNumber = 0;
        SetUpGame();
    }

    private void SetUpGame() {
        CreateCharacters();
        CreateEnemies();
        CreateCells();
        CreateUI();
    }

    private void CreateCells() { //Called in awake to set up game
        playerCell = new Cell[] { new Cell(characterPosition[0], characterEntity[0]), new Cell(characterPosition[1], characterEntity[1]) };
        enemyCell = new Cell[enemyData.Length];
        for (int i = 0; i < enemyData.Length; i++) {
            if (enemyData[i] != null) {
                enemyCell[i] = new Cell(enemyPosition[i], enemyEntity[i]);
            }
            else {
                enemyCell[i] = new Cell(enemyPosition[i]);
            }
        }
    }
    private void CreateCharacters() { //Called in awake to set up game. Create characters from characterData.
        GameObject[] characterGameObject = new GameObject[characterData.Length]; //All of the characterData should be in arrays of 2.
        characterEntity = new Entity[characterData.Length];
        for (int i = 0; i < characterData.Length; i++) {
            characterGameObject[i] = Instantiate(characterPrefab[i], characterPosition[i]);
            characterGameObject[i].GetComponent<SpriteRenderer>().sprite = characterData[i].GetSprite();
            characterEntity[i] = new Entity(characterGameObject[i], characterData[i]);
        }
    }
    private void CreateEnemies() { //Called in awake to set up game. Create enemies from enemyData.
        GameObject[] enemyGameObject = new GameObject[enemyData.Length];
        enemyEntity = new Entity[4];
        for (int i = 0; i < 4; i++) {
            if (i < enemyData.Length) {
                enemyGameObject[i] = Instantiate(enemyPrefab[i], enemyPosition[i]);
                enemyGameObject[i].GetComponent<SpriteRenderer>().sprite = enemyData[i].GetSprite();
                enemyEntity[i] = new Entity(enemyGameObject[i], enemyData[i]);
            }
            else {
                enemyEntity[i] = new Entity(false);
            }
        }
    }
    private void CreateUI() {
        for (int i = 0; i < characterEntity.Length; i++) {
            int health = characterEntity[i].data.GetCurrentVitality();
            characterHealthBar[i].gameObject.SetActive(true);
            characterHealthText[i].enabled = true;
            characterImage[i].enabled = true;
            characterHealthBar[i].maxValue = characterEntity[i].data.GetMaxVitality();
            characterSpBar[i].maxValue = characterEntity[i].data.GetMaxSP();
            SetCharacterHealth(i, characterEntity[i].data.GetMaxVitality());
            SetCharacterSP(i, characterEntity[i].data.GetMaxSP());
        }
        for (int i = 0; i < enemyHealthBar.Length;i++) {
            if (i < GetEnemyCount()) {
                int health = enemyEntity[i].data.GetCurrentVitality();
                enemyHealthBar[i].gameObject.SetActive(true);
                enemyName[i].enabled = true;
                enemyImage[i].enabled = true;
                enemyHealthBar[i].maxValue = enemyEntity[i].data.GetMaxVitality();
                enemyName[i].text = ((EnemyData)enemyEntity[i].data).GetName();
                SetEnemyHealth(i, enemyEntity[i].data.GetMaxVitality());
            }
            else {
                enemyHealthBar[i].gameObject.SetActive(false);
                enemyName[i].enabled = false;
                enemyImage[i].enabled = false;
            }
        }
    }

    private void SetCharacterHealth(int characterIndex, int healthSet) {
        characterEntity[characterIndex].data.SetCurrentVitality(healthSet);
        int health = characterEntity[characterIndex].data.GetCurrentVitality();
        characterHealthBar[characterIndex].value = health;
        characterHealthText[characterIndex].text = "HP: " + health;
    }
    private void ModifyCharacterHealth(int characterIndex, int healthModification) {
        characterEntity[characterIndex].data.ModifyCurrentVitality(healthModification);
        int health = characterEntity[characterIndex].data.GetCurrentVitality();
        characterHealthBar[characterIndex].value = health;
        characterHealthText[characterIndex].text = "HP: " + health;
    }
    private void SetCharacterSP(int characterIndex, int spSet) {
        characterEntity[characterIndex].data.SetCurrentSP(spSet);
        int sP = characterEntity[characterIndex].data.GetCurrentSP();
        characterSpBar[characterIndex].value = sP;
        characterSpText[characterIndex].text = "SP: " + sP;
    }
    private void ModifyCharacterSP(int characterIndex, int spModification) {
        characterEntity[characterIndex].data.ModifyCurrentSP(spModification);
        int sP = characterEntity[characterIndex].data.GetCurrentSP();
        characterSpBar[characterIndex].value = sP;
        characterSpText[characterIndex].text = "SP: " + sP;
    }
    private void SetEnemyHealth(int enemyIndex, int healthSet) {
        enemyEntity[enemyIndex].data.SetCurrentVitality(healthSet);
        enemyHealthBar[enemyIndex].value = enemyEntity[enemyIndex].data.GetCurrentVitality();
    }
    private void ModifyEnemyHealth(int enemyIndex, int healthModification) {
        enemyEntity[enemyIndex].data.ModifyCurrentVitality(healthModification);
        enemyHealthBar[enemyIndex].value = enemyEntity[enemyIndex].data.GetCurrentVitality();
    }
    //private void 

    public CharacterData GetCharacterData(bool character) { //True for dale 0, false for gail 1
        return characterData[character ? 0 : 1];
    }
    
    public int GetTurnNumber() {
        return turnNumber;
    }
    public void SetTurnNumber(int number) {
        this.turnNumber = number;
    }
    public void IncreaseTurnNumber(int number) {
        this.turnNumber += number;
    }
    public void IncreaseTurnNumber() {
        this.turnNumber++;
    }
    public int GetEnemyCount() {
        int count = 0;
        foreach (Entity entity in enemyEntity) {
            if (entity.GetAlive()) {
                count++;
            }
        }
        return count;
    }
    public Entity[] GetEnemyEntities() {
        return enemyEntity;
    }
    public Entity GetCharacterEntity(int index) {
        return characterEntity[index];
    }

    public struct Cell {
        //private Entity entity; //null if nothing
        //private Transform position;
        public Entity entity { get; set; }
        public Transform transform { get; set; }

        public Cell(Transform transform, Entity entity) : this() {
            this.transform = transform;
            this.entity = entity;
        }
        public Cell(Transform transform) : this() {
            this.transform = transform;
        }
    }

    public struct Entity {
        public GameObject gameObject { get; set; }
        public EntityData data { get; set; }
        private float highlightModifier;
        private bool alive;
        public Entity(GameObject entityGameObject, EntityData entityData) : this() {
            this.gameObject = entityGameObject;
            this.data = entityData;
            this.highlightModifier = 1.2f;
            alive = true;
        }
        public Entity(bool alive) : this() {
            alive = false;
        }
        public void ToggleSelected(bool selected) {
            if (selected && alive) {
                gameObject.transform.localScale = new Vector3(highlightModifier, highlightModifier, highlightModifier);
            }
            else if (alive) {
                gameObject.transform.localScale = Vector3.one;
            }
        }
        public bool GetAlive() {
            return alive;
        }
    }
}
