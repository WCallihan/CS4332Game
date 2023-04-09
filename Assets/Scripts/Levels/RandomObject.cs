using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour {
	
    [SerializeField] private GameObject[] possibleObjects;

    //choose a random object in array and set it active; deactivate all others
    private void Awake() {
        int rand = Random.Range(0, possibleObjects.Length);
        foreach(var o in possibleObjects) {
            o.SetActive(false);
        }
        possibleObjects[rand].SetActive(true);
    }
}