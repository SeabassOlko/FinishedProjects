using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Controller player look with mouse movement
*/

public class MouseLook : MonoBehaviour
{
    // mouse sensitivity that can be adjusted in the menu
    public float _sensitivity = 500f;
    // mouse x rotation
    float _XRotation = 0f;
    // mouse y rotation
    float _YRotation = 0f;

    void Start() 
    {
        // set sensitivity to that of the menu scroll bar
        _sensitivity = GameObject.Find("_SettingsHolder").GetComponent<SettingsHolder>().sensitivity;
        // lock curser and make it invisible for the game duration
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse input on X and Y axis
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        // clamp player x rotation to 180 degrees
        _XRotation -= mouseY;
        _XRotation = Mathf.Clamp(_XRotation, -90f, 90f);

        // clamp player y rotation to 180 degrees
        _YRotation += mouseX;
        _YRotation = Mathf.Clamp(_YRotation, -90f, 90f);

        // rotate player based on X and Y rotations
        transform.localRotation = Quaternion.Euler(_XRotation, _YRotation, 0f);
    }
}
