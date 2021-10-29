using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: saves the score and level information to be taken between scenes then saved in the score screen
*/

public class ScoreHolder : MonoBehaviour
{
    // variables that are to be saved between scenes
    public int Level = 0;
    public int Score = 0;
    public int HeadShots = 0;
    public int Targets = 0;

    // method that takes Score, Headshots, Targets, and Level and saves them into the scoreholders variables
    public void SaveScores(int scr, int hdSht, int targets, int lvl)
    {
        Score = scr;
        HeadShots = hdSht;
        Targets = targets;
        Level = lvl;
    }

    // method that outputs the saved scores in the same way they were input
    public void GetScores(out int scr, out int hdSht, out int targets, out int level)
    {
        scr = Score;
        hdSht = HeadShots;
        targets = Targets;
        level = Level;
    }
}
