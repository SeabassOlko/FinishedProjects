using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Changes scene once the player has reached the goal at the end of the level
*/

public class Goal : MonoBehaviour
{
    // method takes collider, checks level and if goal has been entered then changes scene relative to level
    void OnTriggerEnter(Collider other) {

        // get current scene
        Scene scene = SceneManager.GetActiveScene();

        // depending on scene it will either go to main menu or change to the next level/win screen
        if (scene.name == "Tutorial")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (scene.name == "Level_1")
        {
            SceneManager.LoadScene("Level_2");
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
        } 
	}
}
