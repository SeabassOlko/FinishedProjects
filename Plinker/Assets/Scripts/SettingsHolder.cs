using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: simple settings holder to handle mouse sensitivity between scenes 
*/

public class SettingsHolder : MonoBehaviour
{   
    // float of the players mouse sensitivity
    public float sensitivity;

    void Start() 
    {
        // set sensitivity to default 100 on game load
        sensitivity = 100;
        // save setting through scenes
        DontDestroyOnLoad(gameObject);
    }

    // method which is called from the scroll bar in the main menu to set the sensitivity
    public void SetSensitivity(float sen)
    {
        sensitivity = sen;
    }

}
