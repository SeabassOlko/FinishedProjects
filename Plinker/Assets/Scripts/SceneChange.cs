using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Control scene changes and give method options for loading scenes
*/

public class SceneChange : MonoBehaviour
{
    // reset curser visability and use on start
    private void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // method takes scene number and changes to that scene
    public void ChangeScene(int sceneChange)
    {   
        SceneManager.LoadScene(sceneChange);
    }
    // method quits the game
    public void Quit()
    {
        Application.Quit();
    }
    // method loads the menu scene
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
