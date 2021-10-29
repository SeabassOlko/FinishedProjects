using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Controls target movement from variables given to the target from the target spawner, and controls animations if the target is hit by the player
*/

public class TargetController : MonoBehaviour
{
    // Tag
    private string TargetType;

    //Targets Controller
    public TargetSpawner Spawner;

    // Animation
    public Animator animator;

    // Hit tracker
    private bool TargetHit = false;

    // Target Movement
    public bool Moving = false;
    public float Distance;

    // Direction: True == left, false == right
    public bool Direction;
    private float MoveDirection;
    public bool FullMove;
    private bool MoveBack = false;
    public float Speed;
    private Vector3 TargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        // get the targettype name
        TargetType = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        // check if target should be moving
        if (Moving)
            Move();
        // if target reached end of movement and should move back then reverse movement
        if (gameObject.transform.position.x >= TargetPosition.x && Direction == false && MoveBack == true)
        {
            Reverse(); 
        }
        // if target reached end of movement and should move back then reverse movement    
        else if (gameObject.transform.position.x <= TargetPosition.x && Direction == true && MoveBack == true)
        {
            Reverse();
        }
        // if target has reached the end of its movement then it can be destroyed
        if (gameObject.transform.position.x >= TargetPosition.x && Direction == false && MoveBack == false)
        {
            Destroy(); 
        }  
        // if target has reached the end of its movement then it can be destroyed  
        else if (gameObject.transform.position.x <= TargetPosition.x && Direction == true && MoveBack == false)
        {
            Destroy();
        }
            
    }

    // method that is called when target is shot
    public void Hit()
    {
        // if target has not already been hit then animate to fall down and set hit to true
        if (!TargetHit)
        {
            TargetHit = true;
            animator.SetBool("Hit", true);
        }
    }

    // method checks if the target is hit for scoring purposes
    public bool IsHit()
    {
        return TargetHit;
    }

    // method moves target depending on what variables are given when the target is spawned
    public void Movement(float Dis, bool Dir, bool FM, float Spd, TargetSpawner Spwn)
    {
        // set variables for movement, and moving to true
        Spawner = Spwn;
        Moving = true;
        Distance = Dis;
        Direction = Dir;
        FullMove = FM;
        // if a full move across screen is false then the target will return back to spawn
        if (FullMove == false)
            MoveBack = true;
        Speed = Spd;

        // if direction is true then set move direction to -1 or left for the player
        if (Direction)
            MoveDirection = -1;
        // if direction is false then set move direction to 1 or right for the player
        else
            MoveDirection = 1;

        // set the target position that it must reach by figuring the distance times the direction then set target position
        Vector3 Temp = new Vector3(Distance * MoveDirection, 0f, 0f);
        TargetPosition = gameObject.transform.position + Temp;
    }

    // method moves the target using movement variables
    void Move()
    {
        // move target left or right depending on Direction, only if it has not passed its targetposition
        if (gameObject.transform.position.x <= TargetPosition.x && Direction == false)
            gameObject.transform.position += new Vector3(Time.deltaTime * Speed, 0f, 0f);
        else if (gameObject.transform.position.x >= TargetPosition.x && Direction == true)
            gameObject.transform.position -= new Vector3(Time.deltaTime * Speed, 0f, 0f);;
    }

    // method reverses the targets movement and it target position to reach
    void Reverse()
    {
        // set moveback to false so it does not reverse again
        MoveBack = false;
        // switch bool direction
        Direction = !Direction;
        // switch movedirection
        MoveDirection = -MoveDirection;
        // set the target position that it must reach by figuring the distance times the direction then set target position
        Vector3 Temp = new Vector3(Distance * MoveDirection, 0f, 0f);
        TargetPosition = gameObject.transform.position + Temp;
    }

    // method destroys game object and tells spawner it can spawn again
    void Destroy()
    {
        // allow spawner to spawn another target
        Spawner.Spawned = false;
        // destroy this target
        Destroy(gameObject);
    }

}
