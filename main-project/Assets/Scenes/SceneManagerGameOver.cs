using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerGameOver : MonoBehaviour {

    [SerializeField] TextMeshProUGUI playAgainText;
    [SerializeField] TextMeshProUGUI mainMenuText;
    bool selected = true;

    public void PlayAgain() {
        SceneManager.LoadScene("Combat01");
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void Update() {
        if (selected) {
            playAgainText.text = "<b>Play Again</b>";
            mainMenuText.text = "Main Menu";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                PlayAgain();
            }
        }
        else {
            playAgainText.text = "Play Again";
            mainMenuText.text = "<b>Main Menu</b>";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                MainMenu();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {
            selected = !selected;
        }
    }


}
