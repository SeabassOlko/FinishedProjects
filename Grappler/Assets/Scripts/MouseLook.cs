using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Controls players mouse look and clamps verticle looking
*/

public class MouseLook : MonoBehaviour
{
    // variable for mouse sensitivity
    public float sensitivity = 250f;

    // player body and rotation
    public Transform playerBody;
    float xRotation = 0f;
    
    // Update is called once per frame
    void Update()
    {
        // get the players mouse x and y and move the player on the y but the camera on the x with clamping
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // clamp verticle look to 90 degrees up or down to not rotate player
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // apply mouse rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}