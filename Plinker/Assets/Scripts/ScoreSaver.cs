using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Saves highscores and displays the score and level information on the score screen
*/

public class ScoreSaver : MonoBehaviour
{
    // Text for the UI to use
    public Text ScoreText;
    public Text HighScoreText;
    public Text HeadshotText;
    
    // private score variables that are taken from score holder
    private int Level;
    private int HighScore;
    private int Score;
    private int HeadShots;
    private int Targets;

    // string that holds the level Highscore tag
    private string LevelString;

    // Score holder game object and component variables
    public GameObject ScoreH;
    public ScoreHolder ScoreHolder;

    void Start() 
    {
        // Find the scoreholder that was loaded into the scene
        ScoreH = GameObject.Find("ScoreHolder");
        // get scoreholder component from scoreH and import the scores
        ScoreHolder = ScoreH.GetComponent<ScoreHolder>();
        ScoreHolder.GetScores(out Score, out HeadShots, out Targets, out Level);

        // Create level Tag for obtaining the highscore from playerPrefs
        LevelString = "HighScore" + Level;
        // Get previous highscore if there is one from playerPrefs
        HighScore = PlayerPrefs.GetInt(LevelString);

        CheckHighscore();
        // convert score Integer to string for the UI
        ScoreText.text = Score.ToString();
        // Convert Highscore integer into string for the UI
        HighScoreText.text = HighScore.ToString();
        // compare the amount of headshots to the amount of targets in the level and convert to string for UI
        HeadshotText.text = HeadShots.ToString() + "/" + Targets.ToString(); 
        // destroy the no longer needed score holder since scores were stored here
        Destroy(ScoreH);
    }


    // method compared new score to Highscore to see which is larger
    void CheckHighscore()
    {
        // if the new score is higher than save score as the new Highscore
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(LevelString, Score);
        }
    }

    // Reset score is called when the UI button reset is pressed and resets the score to 0
    public void ResetScore()
    {
        // Set highscore to 0 and save it in playerPrefs
        HighScoreText.text = "0";
        PlayerPrefs.SetInt(LevelString, 0);
    }


}
