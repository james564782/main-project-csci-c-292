using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] RectTransform[] transitions; //[0 is the up transition, 1 is the down transition]

    [Header("Prefabs")]
    [SerializeField] GameObject[] characterPrefab;
    [SerializeField] GameObject[] enemyPrefab;

    [Header("Other")]
    [SerializeField] string nextLevelName;


    Cell[] playerCell = new Cell[2];
    Cell[] enemyCell = new Cell[4];
    Entity[] characterEntity;
    Entity[] enemyEntity;

    private bool levelComplete = false; //False by default
    private bool gameOver = false; //False by default
    private bool[] transitionActive = new bool[] { false, false }; //If true then it is covering the screen, if false then it is not


    private int turnNumber = 0; //Each time it is the players turn, increase by 1. Starts at 0.

    private void Awake() { //Use this to set up the game.
        system = this;
        stateMachine = GetComponent<CombatStateMachine>();
        turnNumber = 0;
        SetUpGame();
    }

    private void SetUpGame() {
        StartCoroutine(EntryTransition());
        CreateCharacters();
        CreateEnemies();
        CreateCells();
        CreateUI();
        levelComplete = false;
        gameOver = false;
    }

    IEnumerator EntryTransition() {
        transitions[1].gameObject.SetActive(true);
        Vector3 startPosition = new Vector3(0, -500, 0);
        Vector3 endPosition = new Vector3(0, 500, 0);
        transitionActive[1] = true;
        for (float i = 0; i < 1f; i += Time.deltaTime / 0.75f) {
            transitions[1].localPosition = Vector3.Lerp(startPosition, endPosition, i);
            yield return null;
        }
        transitionActive[1] = false;
    }

    public bool GetLevelComplete() {
        return levelComplete;
    }
    public bool GetGameOver() {
        return gameOver;
    }
    public void SetLevelComplete(bool value) { //To set the level as complete
        if (value) {
            levelComplete = value;
            stateMachine.SetEnded(value);
            StartCoroutine(TransitionToScene(nextLevelName, 0.75f, 0.5f));
        }
    }
    public void SetGameOver(bool value) { //To set the game as over.
        if (value) {
            gameOver = value;
            stateMachine.SetEnded(value);
            StartCoroutine(TransitionToScene("Gameover", 0.75f, 0.5f));
        }
    }
    IEnumerator TransitionToScene(string nextScene, float time, float initialPause) {
        transitions[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(initialPause);
        Vector3 startPosition = new Vector3(0, -500, 0);
        Vector3 endPosition = new Vector3(0, 500, 0);
        transitionActive[0] = true;
        for (float i = 0; i < 1f; i += Time.deltaTime / time) {
            transitions[0].localPosition = Vector3.Lerp(startPosition, endPosition, i);
            yield return null;
        }
        SceneManager.LoadScene(nextScene);
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
            characterEntity[i] = new Entity(characterGameObject[i], characterData[i], i);
        }
    }
    private void CreateEnemies() { //Called in awake to set up game. Create enemies from enemyData.
        GameObject[] enemyGameObject = new GameObject[enemyData.Length];
        enemyEntity = new Entity[4];
        for (int i = 0; i < 4; i++) {
            if (i < enemyData.Length) {
                enemyGameObject[i] = Instantiate(enemyPrefab[i], enemyPosition[i]);
                enemyGameObject[i].GetComponent<SpriteRenderer>().sprite = enemyData[i].GetSprite();
                enemyEntity[i] = new Entity(enemyGameObject[i], enemyData[i], i);
                enemyGameObject[i].GetComponent<Animator>().Play("Sentry01Idle", 0, (i % 2f) / 2f);
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
            ModifyCharacterSPBar(i);
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

    public void SetCharacterHealth(int characterIndex, int healthSet) {
        characterEntity[characterIndex].data.SetCurrentVitality(healthSet);
        int health = characterEntity[characterIndex].data.GetCurrentVitality();
        characterHealthBar[characterIndex].value = health;
        characterHealthText[characterIndex].text = "HP: " + health;
        SetGameOver(IsGameOver());
    }
    public void ModifyCharacterHealth(int characterIndex, int healthModification) {
        characterEntity[characterIndex].ModifyHealth(healthModification);
        int health = characterEntity[characterIndex].GetCurrentHealth();
        characterEntity[characterIndex].data.SetCurrentVitality(health);
        characterHealthBar[characterIndex].value = health;
        characterHealthText[characterIndex].text = "HP: " + health;
        SetGameOver(IsGameOver());
    }

    public bool IsLevelComplete() { //If at least one enemy is still alive then the level isn't over
        foreach (Entity entity in enemyEntity) {
            if (entity.GetAlive()) {
                return false;
            }
        }
        return true;
    }
    public bool IsGameOver() { //If at least one character is still alive then the game isn't over
        foreach (Entity entity in characterEntity) {
            if (entity.GetAlive()) {
                return false;
            }
        }
        return true;
    }

    public void ModifyCharacterSPBar(int characterIndex) {
        int sP = characterEntity[characterIndex].GetSP();
        characterSpBar[characterIndex].value = sP;
        characterSpText[characterIndex].text = "SP: " + sP;
    }
    public void ModifyCharacterSPBar(int characterIndex, int newSP) {
        characterSpBar[characterIndex].value = newSP;
        characterSpText[characterIndex].text = "SP: " + newSP;
    }
    public void SetEnemyHealth(int enemyIndex, int healthSet) { //This method probably doesn't work well, look at ModifyEnemyHealth
        enemyEntity[enemyIndex].data.SetCurrentVitality(healthSet);
        enemyHealthBar[enemyIndex].value = enemyEntity[enemyIndex].data.GetCurrentVitality();
        SetLevelComplete(IsLevelComplete());
    }
    public void ModifyEnemyHealth(int enemyIndex, int healthModification) { //Can't modify the enemyData health since that corresponds to all enemies.
        enemyEntity[enemyIndex].ModifyHealth(healthModification);
        enemyHealthBar[enemyIndex].value = enemyEntity[enemyIndex].GetCurrentHealth();
        SetLevelComplete(IsLevelComplete());
    }
    public void InflictBleedingToEnemy(int index, int turnCount, int damagePerTurn) {
        enemyEntity[index].InflictBleed(turnCount, damagePerTurn);
    }
    public void ResolveAllEnemyBleeding() {
        foreach (Entity e in enemyEntity) {
            if (e.GetAlive()) {
                e.Bleed();
            }
        }
    }

    public int GetAlivePlayersCount() {
        int alive = 0;
        if (characterEntity[0].GetAlive()) {
            alive++;
        }
        if (characterEntity[1].GetAlive()) {
            alive++;
        }
        return alive;
    }

    public CharacterData GetCharacterData(bool character) { //True for dale 0, false for gail 1
        return characterData[character ? 0 : 1];
    }
    public GameObject GetCharacterGameObject(bool character) {
        return characterEntity[character ? 0 : 1].gameObject;
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

    public void SetCharacterImageSelected(int index, bool value) {
        if (value) {
            characterImage[index].color = new Color(1, 0, 0);
        }
        else {
            characterImage[index].color = new Color(1, 1, 1);
        }
    }
    public void SetEnemyImageSelected(int index, bool value) {
        if (value) {
            enemyImage[index].color = new Color(1, 0, 0);
        }
        else {
            enemyImage[index].color = new Color(1, 1, 1);
        }
    }

    public void CharacterAttack(bool character) {
        StartCoroutine(AnimateCharacterAttack(character ? 0 : 1));
    }

    IEnumerator AnimateCharacterAttack(int characterIndex) { //Trigger this after the quick time event
        float rate = 3f;
        float horizontalDistance = 1.0f;
        float[] verticalDistance = new float[] { 0.1f, 0.25f };
        GameObject characterObject = characterEntity[characterIndex].gameObject;
        Vector3 startingPosition = characterPosition[characterIndex].position;
        for (float i = 0; i < 1; i += Time.deltaTime * rate) {
            float valueA = (-Mathf.Pow(i - 0.5f, 2) * 4 + 1);
            float[] valueB = new float[] { (-Mathf.Pow((i * 2f) - 0.5f, 2) * 4 + 1), (-Mathf.Pow(((i * 2f) - 1) - 0.5f, 2) * 4 + 1) };
            float x = Mathf.Lerp(startingPosition.x, startingPosition.x + horizontalDistance, valueA);
            float y = i < 0.5f ? 
                Mathf.Lerp(startingPosition.y, startingPosition.y + verticalDistance[0], valueB[0]) : 
                Mathf.Lerp(startingPosition.y, startingPosition.y + verticalDistance[1], valueB[1]);
            characterObject.transform.position = new Vector3(x, y, 0);
            yield return null;
        }
        characterObject.transform.position = characterPosition[characterIndex].position;
    }

    public Entity[] GetEnemyEntities() {
        return enemyEntity;
    }
    public Entity GetCharacterEntity(int index) {
        return characterEntity[index];
    }

    public void TurnIconBarsInvisible(bool isEnemy, int count) {
        if (isEnemy && count < 4) {
            enemyHealthBar[count].gameObject.SetActive(false);
            enemyImage[count].gameObject.SetActive(false);
            enemyName[count].gameObject.SetActive(false);
        }
        else if (!isEnemy && count < 2) {
            characterHealthBar[count].gameObject.SetActive(false);
            characterSpBar[count].gameObject.SetActive(false);
            characterHealthText[count].gameObject.SetActive(false);
            characterSpText[count].gameObject.SetActive(false);
            characterImage[count].gameObject.SetActive(false);
        }
    }

    public void SpendSP(int characterIndex, int spent) {
        characterEntity[characterIndex].SpendSP(spent);
    }

    public struct Cell { //Didn't really end up using this
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
        private int index;
        private float highlightModifier;
        private int max_health;
        private int current_health;
        private bool alive;
        private int[] bleed; //[damage, turncount]
        private int sp { get; set; }
        public Entity(GameObject entityGameObject, EntityData entityData, int index) : this() {
            this.index = index;
            this.gameObject = entityGameObject;
            this.data = entityData;
            this.highlightModifier = 1.2f;
            this.max_health = entityData.GetMaxVitality();
            this.current_health = this.max_health;
            alive = true;
            this.bleed = new int[] { 0, 0 };
            this.sp = data.GetMaxSP();
        }
        public Entity(bool alive) : this() {
            alive = false;
            this.bleed = new int[] { 0, 0 };
        }
        public void ToggleSelected(bool selected) {
            if (selected && alive) {
                gameObject.transform.localScale = new Vector3(highlightModifier, highlightModifier, highlightModifier);
            }
            else if (alive) {
                gameObject.transform.localScale = Vector3.one;
            }
        }
        public void ModifyHealth(int modified_Vitality) {
            current_health = Mathf.Clamp(current_health + modified_Vitality, 0, max_health);
            if (current_health <= 0 || !alive) {
                alive = false;
                AnimateDeath();
            }
            else {
                AnimateDamage();
            }
        }

        public void AnimateDamage() {
            if (data is EnemyData) {
                gameObject.GetComponent<Animator>().Play("Sentry01Damage", 0, 0.01f);
            }
        }
        public void AnimateDeath() {
            if (data is EnemyData) {
                gameObject.GetComponent<Animator>().Play("Sentry01Death", 0, 0.01f);
                CombatSystem.system.TurnIconBarsInvisible(true, index);
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                CombatSystem.system.TurnIconBarsInvisible(false, index);
            }
        }
        public void InflictBleed(int turnCount, int damage) {//Giving the effect
            this.bleed = new int[] { turnCount, damage };
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
        }
        public void Bleed() { //Damage each turn
            if (bleed[0] > 0) {
                CombatSystem.system.ModifyEnemyHealth(index, -bleed[1]);
                bleed[0]--;
            }
            if (bleed[0] > 0) {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            }
        }
        public void SpendSP(int value) {
            SetSP(sp - value);
            CombatSystem.system.ModifyCharacterSPBar(index, sp);
        }
        public int GetSP() {
            return sp;
        }
        public void SetSP(int newValue) {
            sp = Mathf.Clamp(newValue, 0, data.GetMaxSP());
        }
        public int GetMaxSP() {
            return data.GetMaxSP();
        }
        public int GetCurrentHealth() {
            return current_health;
        }
        public bool GetAlive() {
            return alive;
        }
    }
}
