using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Controls spawning of targets and hostages based on random variable given to each spawned target, and gives the targets movement directions and variables
*/

public class TargetSpawner : MonoBehaviour
{
    // bool for if spawner already has a target spawned
    public bool Spawned = false;

    // two stored prefabs for spawning
    public GameObject TargetPrefab;
    public GameObject HostagePrefab;

    // game controller for tracking how many targets can be spawned
    public GameController gameController;

    void Start() 
    {
        // get game controller from scene
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    void Update() 
    {
        // if there is still more than one target left in the count then spawn target
        if (gameController.TargetAmount >= 1 && Spawned == false)
        {
            Spawn();
        }
    }

    // method spawns target with random movement and possible hostage target
    void Spawn()
    {
        // set bool to true
        Spawned = true;
        //reduce target amount counter
        gameController.TargetAmount--;

        // holder variables for either a full move, or a given distance
        bool FullMove;
        float Distance;
        // get a random move speed between range
        float Speed = Random.Range(0.75f, 2f);
        // get random direction from method true == left, false == right
        bool Direction = RdmDirection();

        // call method for random move distance
        RdmDistance(out FullMove, out Distance);

        // calculate distance (distance is % of map width)
        Distance = Distance * gameController.MapWidth;

        // if moving right then spawn target on left side of target range
        if (Direction == false)
        {
            // spawn target prefab at target spawner position
            GameObject Target = Instantiate(TargetPrefab, transform.position, transform.rotation);
            // give the target its movement variables after being spawned
            Target.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            
            // possibly spawn hostage with target
            if (RdmHostage())
            {
                // spawn hostage slightly beside the target to make the shot harder
                GameObject Hostage = Instantiate(HostagePrefab, transform.position - new Vector3(Random.Range(.3f, .5f), 0, 0.1f), transform.rotation);
                // give hostage same movement as the main target
                Hostage.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            }
        }
        // else moving left then spawn target on right side of target range
        else
        {
            // position when spawning from right is the target spawner position plus the map width
            Vector3 position = transform.position + new Vector3(gameController.MapWidth, 0, 0);

            // spawn target on other side or target range from target spawner
            GameObject Target = Instantiate(TargetPrefab, position, transform.rotation);
            // give the target its movement variables after being spawned
            Target.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            
            // possibly spawn hostage with target
            if (RdmHostage())
            {
                // spawn hostage slightly beside the target to make the shot harder
                GameObject Hostage = Instantiate(HostagePrefab, position + new Vector3(Random.Range(.3f, .5f), 0, -0.1f), transform.rotation);
                // give hostage same movement as the main target
                Hostage.GetComponent<TargetController>().Movement(Distance, Direction, FullMove, Speed, gameObject.GetComponent<TargetSpawner>());
            }
        }

    }

    // method gets random distance and movement within four posibilities, move just over 1/4 map width, 1/2 map width, 3/4 map width, or full move across map width
    void RdmDistance( out bool FullMove, out float Distance)
    {
        // set temporary variables for out 
        FullMove = false;
        Distance = 2;

        // get random number between 0 and 3
        int random = Random.Range(0,4);

        // first is just over 1/4 map width movement
        if (random == 0)
        {
            FullMove = false;
            Distance = 0.35f;
        }
        // second is 1/2 map width movement
        else if (random == 1)
        {
            FullMove = false;
            Distance = 0.5f;
        }
        // third is 3/4 map width movement
        else if (random == 2)
        {
            FullMove = false;
            Distance = 0.75f;
        }
        // fourth and final is full movement across map width
        else if (random == 3)
        {
            FullMove = true;
            Distance = 1f;
        }
    }

    // method gets random direction either 0 or 1, left or right
    bool RdmDirection()
    {
        return Random.Range(0,2) == 0;
    }

    // method give 10% chance of hostage being spawned
    bool RdmHostage()
    {
        return Random.value < .1;
    }
}
