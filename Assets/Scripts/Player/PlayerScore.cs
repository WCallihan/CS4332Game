using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore instance;

    public TextMeshProUGUI scoreText;

    int score = 0;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Rooms Cleared: " + score.ToString();   
    }

    // Add 1 to player's score
    public void AddPoint()
    {
        score += 1;
        scoreText.text = "Rooms Cleared: " + score.ToString();
    }
}
