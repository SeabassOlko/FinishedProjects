using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: code for UI to get information on score and ammo from game controller and Gun
*/

public class PlayerUI : MonoBehaviour
{
    // temporary object holders for gun and controller
    private GameObject GameControllerObject;
    public GameObject GunObj;

    // gun component
    private Gun Gun;
    // gamecontroller component
    private GameController gameController;

    // text holder for the ammo and score
    public Text Ammo;
    public Text Score;

    void Start() 
    {
        // get the gamecontroller object
        GameControllerObject = GameObject.Find("GameController");
        // get game controller component
        gameController = GameControllerObject.GetComponent<GameController>();
        // get gun component from gun object
        Gun = GunObj.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        // display ammo and score to UI canvas
        Ammo.text = "Ammo: " +Gun.CurrentAmmo;
        Score.text = "Score: " +gameController.Score;
    }
}
