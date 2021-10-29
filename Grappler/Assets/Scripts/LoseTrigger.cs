using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Checks if player has hit a lose obstacle, if true then change scene to lose screen
*/

public class LoseTrigger : MonoBehaviour
{
    // collider put on death objects such as lava or off the map cause scene change to lose screen
    void OnTriggerEnter(Collider other) {
        SceneManager.LoadScene("LoseScreen");  
	}
}
