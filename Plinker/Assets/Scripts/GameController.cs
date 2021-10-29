using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: store player score and map information for other mothods to take information from
*/
public class GameController : MonoBehaviour
{
    // public game object to hold the score for later scenes
    public GameObject ScoreH;
    // scoreholder variable holds level information and player score
    private ScoreHolder ScoreHolder;

    // public serialized score for debugging
    [SerializeField]
    public int Score;
    // private headshot counter
    [SerializeField]
    private int Headshots;

    // public serialized target amount for debugging
    [SerializeField]
    public int TargetAmount;
    private int LevelTargetAmount;

    // current level
    private int Level;

    // map width in Unity Units
    [SerializeField]
    public float MapWidth;

    // sounds for different target hits
    public AudioSource _HeadShot;
    public AudioSource _BodyShot;

    private void Start() 
    {
        // reset score and headshot counter
        Score = 0;
        Headshots = 0;
        // get the scene scoreholder component
        ScoreHolder = ScoreH.GetComponent<ScoreHolder>();
        // get current level
        Scene scene = SceneManager.GetActiveScene();

        // If statements that match the level level name to the current scene and give level details to variables
        if (scene.name == "TestScene")
        {
            MapWidth = 10;
            TargetAmount = 3;
            Level = 0;
        }
        else if (scene.name == "Level_1")
        {
            MapWidth = 10;
            TargetAmount = 15;
            Level = 1;
        }
        else if (scene.name == "Level_2")
        {
            MapWidth = 10;
            TargetAmount = 30;
            Level = 2;
        }
        // set the level target amount
        LevelTargetAmount = TargetAmount;
    }

    void Update() 
    {
        // check if all targets have been shot or moved off screen to end level
        if (TargetAmount == 0)
        {
            if (GameObject.FindGameObjectsWithTag("Target").Length == 0)
            {
                EndLevel();
            }
        }
    }

    // headshot method to adjust score and counter
    public void HeadShot()
    {
        _HeadShot.Play();
        Score += 3;
        Headshots ++;
    }
    // body hit method to adjust score
    public void Hit()
    {
        _BodyShot.Play();
        Score ++;
    }
    // hostage hit method to adjust score negatively
    public void Hostage()
    {
        _BodyShot.Play();
        Score -= 5;
    }
    // end level mothod that saves the level information and score/headshots then loads scorescreen
    void EndLevel()
    {
        ScoreHolder.SaveScores(Score, Headshots, LevelTargetAmount, Level);
        DontDestroyOnLoad(ScoreH);
        SceneManager.LoadScene("ScoreScreen");
    }
}
