using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Hold character variables and take player input to then do movement controls, speed clamping, including jump and crouch checks
*/

public class CharacterMovement : MonoBehaviour
{
    // player scripts and bodies
    private Rigidbody playerBody;
    public Transform orientation;
    private static GroundCheck gCheck;

    private static CrouchHeadCheck headCheck;

    // get gun for scaling with crouching
    private static GrapplingGun gGun;
    public Transform gun;

    // movement variables
    public float playerGravity = 35f;
    public float moveSpeed = 4500f; 
    public float currentMaxSpeed;
    public float currentSpeed;
    public float maxSpeed = 10f;
    public float swingMaxSpeed = 25f;
    public float maxAirSpeed = 5f;
    public float airSpeed = 1500f;
    public float crouchSpeed = 4f;
    public float jumpHeight = 150f;
    public bool onGround;
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;

    public LayerMask levelGround;

    // player size
    private float normPlayerHeight = 1.5f;
    private float crouchPlayerHeight = 0.75f;

    // gun size
    private float gunSize = 0.075f;

    //playerInput
    private float x, z;
    private bool jumping, crouching;

    // on awake get the players rigidbody component
    private void Awake() => playerBody = GetComponent<Rigidbody>();

    // on start get children components of the player and lock the cursor and make it invisible
    private void Start() {
        gCheck = GetComponentInChildren<GroundCheck>();
        headCheck = GetComponentInChildren<CrouchHeadCheck>();
        gGun = GetComponentInChildren<GrapplingGun>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // Update is called once per frame
    private void Update() 
    {
        playerInput();
        ClampPlayerSpeed();
    }
    
    private void FixedUpdate()
    {
        // if the player is crouching/ holding C or control
        if (crouching)
        {
            // call crouch method to scale player down
            Crouch();
            // set new slower movement speed
            currentMaxSpeed = crouchSpeed;
            // check movement
            Movement();
        }   
        else
        {
            // if players head has clearance then uncrouch
            if (!headCheck.IsHeadClearance())
            {
                UnCrouch();
            }
            // if player is on the ground
            if (gCheck.IsGrounded())
            {
                // set speed to max speed for ground movement
                currentMaxSpeed = maxSpeed;
                // check movement
                Movement();
            }
            // else if player is in the air
            else
            {
                // set speed to max speed for air movement
                currentMaxSpeed = maxAirSpeed;
                // check movement
                Movement();
            }
        }
        // if player presses space
        if (jumping)
        {
            jump();
        }

    }

    private void Movement()
    {  
        //Set max speed
        float maxSpeed = this.currentMaxSpeed;

        // add extra gravity to the player
        playerBody.AddForce(Vector3.down * Time.deltaTime * playerGravity);

        // Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        // If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (gCheck.IsGrounded())
        {
            currentSpeed = moveSpeed;

            // Counteract sliding and sloppy movement
            CounterMovement(x, z, mag);

            // check whether adding speed will bring player over max speed
            if (x > 0 && xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (z > 0 && yMag > maxSpeed) z = 0;
            if (z < 0 && yMag < -maxSpeed) z = 0;
        }
        // else if speed is larger than max air speed then cancel the input to not go voer the speed limit
        else
        {
            currentSpeed = airSpeed;

            // only if there is no rope limit air move speed
            if (!gGun.IsRope())
            {
                if (x > 0 && xMag > maxSpeed) x = 0;
                if (x < 0 && xMag < -maxSpeed) x = 0;
                if (z > 0 && yMag > maxSpeed) z = 0;
                if (z < 0 && yMag < -maxSpeed) z = 0;
            }
        }

        //Apply forces to playerBody
        playerBody.AddForce(orientation.transform.forward * z * currentSpeed * Time.deltaTime);
        playerBody.AddForce(orientation.transform.right * x * currentSpeed * Time.deltaTime);
    }

    // method adds verticle force to player for jumping
    private void jump()
    {
        // check if player is on the ground to jump
        if (gCheck.IsGrounded())
        {
            // add verticle force to make the player jump
            playerBody.AddForce(orientation.transform.up * jumpHeight);
        }
    }

    // method checks player input
    private void playerInput()
    {
        // check x and z axis movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        // check if player hits spacebar
        jumping = Input.GetButton("Jump");
        // check if player crouches with C or left Control
        crouching = Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl);
    }

    // method gets the players directional speed to be able to limit speed based on max speed
    public Vector2 FindVelRelativeToLook() 
    {
        // players current forward angle
        float lookAngle = orientation.transform.eulerAngles.y;
        // players angle of movement with 0 being forward
        float moveAngle = Mathf.Atan2(playerBody.velocity.x, playerBody.velocity.z) * Mathf.Rad2Deg;

        // finds the relative velocity angle compared to the moveAngle
        float velY = Mathf.DeltaAngle(lookAngle, moveAngle);
        // the x velocity angle is just 90 degrees away
        float velX = 90 - velY;


        // multiply the magnitude by the angle to get magnitude in each direction
        float magnitude = playerBody.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(velY * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(velX * Mathf.Deg2Rad);
        
        // return directional magnitude
        return new Vector2(xMag, yMag);
    }

    // method applies counter movement to better stop the players movement and prevent sliding
    private void CounterMovement(float x, float y, Vector2 mag) 
    {
        //Counter movement based on direction of movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) 
        {
            playerBody.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) 
        {
            playerBody.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        // Limit the speed of diagonal running to the maxSpeed
        if (Mathf.Sqrt((Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.z, 2))) > currentMaxSpeed) 
        {
            // save the falling speed
            float tempFallspeed = playerBody.velocity.y;
            // limit the speed of the player
            Vector3 normSpeed = playerBody.velocity.normalized * currentMaxSpeed;
            // set player speed but keep the falling speed the same
            playerBody.velocity = new Vector3(normSpeed.x, tempFallspeed, normSpeed.z);
        }
    }

    // method scales the player and the grapple gun size down for crouching
    private void Crouch()
    {
        transform.localScale = new Vector3(1, crouchPlayerHeight, 1);
        gun.transform.localScale = new Vector3(gunSize, gunSize * 2, gunSize);
    }

    // method scales the player and grapple gun back up when uncrouched
    private void UnCrouch()
    {
        transform.localScale = new Vector3(1, normPlayerHeight, 1);
        gun.transform.localScale = new Vector3(gunSize, gunSize, gunSize);
    }

    // method clamps the players swing speed to the max swing speed
    private void ClampPlayerSpeed()
    {
        // Limit the player swing speed to the max airspeed
        if (Mathf.Sqrt((Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.z, 2))) > swingMaxSpeed) 
        {
            // save the falling speed
            float tempFallspeed = playerBody.velocity.y;
            // limit the speed of the player
            Vector3 normSpeed = playerBody.velocity.normalized * swingMaxSpeed;
            // set player speed but keep the falling speed the same
            playerBody.velocity = new Vector3(normSpeed.x, tempFallspeed, normSpeed.z);
        }
    }
}