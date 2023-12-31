using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : MonoBehaviour
{
    CombatState[] states;
    CombatState currentState;
    bool selectedCharacter = true; //true for dale, false for gail
    int playerTurns = 2;
    [SerializeField] AudioClip selectionUISound;

    bool ended = false;

    private void Awake() {
        InitializeVariables();
    }

    private void InitializeVariables() {
        states = GetComponents<CombatState>();
        ended = false;
        foreach (CombatState state in states) {
            state.SetCombatStateMachine(this);
            if (state is PlayerActionSelection) {
                currentState = state;
            }
        }
    }
    public bool GetEnded() { //Has the game/level ended
        return ended;
    }
    public void SetEnded(bool value) {
        ended = value;
    }

    public void PlaySelectionUISound() {
        SoundBoard.instance.PlayAudio(selectionUISound);
    }
    public bool GetCharacter() { //true for dale, false for gail
        return selectedCharacter;
    }
    public CombatSystem.Entity GetCharacterEntity() {
        return CombatSystem.system.GetCharacterEntity(GetCharacter() ? 0 : 1);
    }
    public int GetCharacterNumber() {
        return selectedCharacter ? 0 : 1;
    }
    public CharacterData GetCharacterData() {
        return CombatSystem.system.GetCharacterData(selectedCharacter);
    }
    public CharacterData GetCharacterData(bool selectedCharacter) {
        return CombatSystem.system.GetCharacterData(selectedCharacter);
    }
    public Skill[] GetSkillList() {
        return GetCharacterData().GetEquippedSkills();
    }
    public Skill[] GetSkillList(bool selectedCharacter) {
        return GetCharacterData(selectedCharacter).GetEquippedSkills();
    }
    public void ChangeCharacter(bool character) { //true for dale, false for gail
        selectedCharacter = character;
        HighlightCharacter(true);
    }
    public void ChangeCharacter() { //true for dale, false for gail
        selectedCharacter = !selectedCharacter;
        HighlightCharacter(true);
    }
    public void HighlightCharacter() {
        HighlightCharacter(true);
    }
    public void SpendSP(int sp) {
        GetCharacterEntity().SpendSP(sp);
        CombatSystem.system.SpendSP(GetCharacterNumber(), sp);
    }

    public void EndPlayerTurn() {
        playerTurns--;
        selectedCharacter = !selectedCharacter;
        if (playerTurns <= 0) {
            playerTurns = 2;
            HighlightCharacter(false);
            ChangeState("EnemyAttack");
        }
        else {
            HighlightCharacter(true);
            ChangeState("PlayerActionSelection");
        }
    }

    public void HighlightCharacter(bool value) { //true if highlight selected character, false if don't highlight anything/enemies turn
        if (value) {
            CombatSystem.system.GetCharacterGameObject(selectedCharacter).GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f);
            CombatSystem.system.GetCharacterGameObject(!selectedCharacter).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        else {
            CombatSystem.system.GetCharacterGameObject(true).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            CombatSystem.system.GetCharacterGameObject(false).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    public void StartPlayerTurn() { //Call this method after enemy turn ends
        CombatSystem.system.IncreaseTurnNumber(1);
    }

    public void ChangeState(string stateType) { //Communicates with CombatState.ChangeState();
        if (!ended) {
            currentState = GetState(stateType);
            //Debug.Log("Changed to state: " + currentState.GetType());

            try {
                currentState.StateStart();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }

    public void ChangeState(string stateType, Skill skill) { //Communicates with CombatState.ChangeState(); If using skill, then that is used here too
        if (!ended) {
            currentState = GetState(stateType);
            ((PlayerSkillUsage)currentState).SetSkill(skill); //This should only be used with skillUsage
                                                              //Debug.Log("Changed to state: " + currentState.GetType() + " of " + skill.name);

            try {
                currentState.StateStart();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }

    private CombatState GetState(string stateType) {
        foreach (CombatState state in states) {
            if (state.GetType() == Type.GetType(stateType)) {
                return state;
            }
        }
        Debug.Log("Can't Get State/State does not exist");
        return currentState;
    }

    private void Start() {
        ChangeCharacter(true); //Set dale to starting character when game starts
        try {
            currentState.StateStart();
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }
    private void Update() {
        if (!ended) {
            try {
                currentState.StateUpdate();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
    private void FixedUpdate() {
        if (!ended) {
            try {
                currentState.StateFixedUpdate();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }


}
