using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerMainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playText;
    [SerializeField] TextMeshProUGUI quitText;
    bool selected = true;
    public void StartGame() {
        SceneManager.LoadScene("Combat01");
    }

    public void QuitGame() {
        Application.Quit();
    }
    public void Update() {
        if (selected) {
            playText.text = "<b>Play</b>";
            quitText.text = "Quit";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                StartGame();
            }
        }
        else {
            playText.text = "Play";
            quitText.text = "<b>Quit</b>";
            if (Input.GetKeyUp("z") || Input.GetKeyUp("space")) {
                QuitGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {
            selected = !selected;
        }
    }


}
