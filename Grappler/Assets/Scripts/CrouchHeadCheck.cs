using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Checks if space above players head is clear and player can stand
*/

public class CrouchHeadCheck : MonoBehaviour
{
    // variable for distance above players head
    public float checkDist = 0.8f;

    // mask layers for checking map terrain
    public LayerMask groundMask;
    public LayerMask grappleMask;

    // method checks if space above players head has no terrain and returns true if clear
    public bool IsHeadClearance()
    {
        // create a sphere to check if players head is clear of colliders and can stand
        if (Physics.CheckSphere(transform.position, checkDist, groundMask) || Physics.CheckSphere(transform.position, checkDist, grappleMask))
        {
            return true;
        }
        return false;
    }
}
