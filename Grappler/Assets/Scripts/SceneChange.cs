using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Controls scene changes and quiting of the game from the menu
*/

public class SceneChange : MonoBehaviour
{
    private void Start() {
        // on start make sure cursor is visible in the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // method takes and integer to change scene to scene that was numbered
    public void ChangeScene(int sceneChange)
    {   
        SceneManager.LoadScene(sceneChange);
    }

    // method quits game when quit is pressed
    public void Quit()
    {
        Application.Quit();
    }
}
