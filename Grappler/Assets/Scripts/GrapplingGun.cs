using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Shoots raycast to targets and creates a spring joint with a drawn rope if target is a grapple surface
*/

public class GrapplingGun : MonoBehaviour
{
    // variables for the linerenderer and grapple gun points
    private LineRenderer lineR;
    private Vector3 grapplePoint;
    private static SpringJoint joint;

    // max length of grapple
    private float maxDistance = 35f;

    // variable for what surface can be hit
    public LayerMask grappleSurface;
    // transforms for reference
    public Transform gunTip, camera, player;
    public Rigidbody playerBody;

    

    private void Awake() 
    {
        lineR = GetComponent<LineRenderer>();
    }

    void Update() 
    {
        // if player presses left mouse button start grapple
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        // else if mouse button is up then stop grapple
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate() 
    {
        DrawRope();
    }

    // method "shoots" grapple and checks for raycast hit and draws grapple if it hits
    void StartGrapple()
    {
        RaycastHit hit;

        // if raycast hits a grapple surface
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, grappleSurface))
        {
            // connect joint to grapple target
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance the grapple will keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // Spring joint values that can be changed for different effects
            joint.spring = 7f;
            joint.damper = 12f;
            joint.massScale = 6f;

            lineR.positionCount = 2;
        }
    }

    // method draws the line that shows the grapple to the target
    void DrawRope()
    {
        // dont draw grapple if no joint
        if (!joint) return;

        lineR.SetPosition(0, gunTip.position);
        lineR.SetPosition(1, grapplePoint);
    }

    // method deletes the spring joint and the drawn line
    void StopGrapple()
    {
        lineR.positionCount = 0;
        Destroy(joint);
    }

    // method returns if there is a spring joint active
    public bool IsRope()
    {
        return joint;
    }

}