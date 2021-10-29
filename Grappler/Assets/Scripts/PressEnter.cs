using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Checks if player pressed enter to then change scene to main menu
*/

public class PressEnter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
      // checks if player pressed enter to change scene
		  if (Input.GetAxis("Submit") == 1) {	
		    SceneManager.LoadScene("MainMenu");
		  }
    }
}
