 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Checks if player is on the ground, returning true if they are
*/

public class GroundCheck : MonoBehaviour
{   
    // check distance from ground
    public float checkDist = 0.4f;
    // ground layer masks that can be stood on
    public LayerMask groundMask;
    public LayerMask grappleMask;

    // method checks if player is standing on a "ground" layermask object
    public bool IsGrounded()
    {
        // create a sphere and check if the player is on the ground, if player is on ground return true
        if (Physics.CheckSphere(transform.position, checkDist, groundMask) || Physics.CheckSphere(transform.position, checkDist, grappleMask))
        {
            return true;
        }
        return false;
    }
}
