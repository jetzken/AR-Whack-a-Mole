using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Score_Script : MonoBehaviour {

    private static int score;
    public Text scoreText;
    public Text finalScoreText;

    public static int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    // Use this for initialization
    void Start () {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score: " + score.ToString();
        finalScoreText.text = "Final Score: " + score.ToString();
	}
}
