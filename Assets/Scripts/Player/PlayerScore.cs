using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;
    public int Score => score; //public getter

    //singleton instance variables
    private static PlayerScore instance;
    public static PlayerScore Instance { get { return instance; } }

    private void OnEnable() {
        LevelChanger.MenuStarted += DestroyPlayer;
    }

    private void OnDisable() {
        LevelChanger.MenuStarted -= DestroyPlayer;
    }


    private void Awake() {
        score = 0;

        //singleton logic
        DontDestroyOnLoad(gameObject);
        if(instance != null && instance != this) {
            //move the already exsisting player to this player's spot; used when starting a new level
            Instance.gameObject.transform.position = gameObject.transform.position;
            Instance.gameObject.transform.rotation = gameObject.transform.rotation;
            //destroy this player
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        scoreText.text = "Rooms Cleared: " + score.ToString();
    }

    // Add 1 to player's score
    public void AddPoint() {
        score += 1;
        scoreText.text = "Rooms Cleared: " + score.ToString();
    }

    //destroys the player game object; used when the main menu is launched
    private void DestroyPlayer() {
        Destroy(gameObject);
    }
}