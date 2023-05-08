using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is placed on the Main Camera to have the player look around with the mouse

public class MouseLook : MonoBehaviour {

    [SerializeField] Transform playerBody;
    [SerializeField] private float mouseSensitivity = 3f;

    private float xRotation = 0f;
    private bool canLook;

    private void OnEnable() {
        UIManager.GamePaused += ToggleLooking;
    }

    private void OnDisable() {
        UIManager.GamePaused -= ToggleLooking;
    }

    private void Awake() {
        canLook = true;
    }

    void Update() {
        if(!canLook) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //clamps the up and down rotation between -90 and 90 degrees

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //looks up and down
        playerBody.Rotate(Vector3.up * mouseX); //rotates the entire player
    }

    private void ToggleLooking(bool gamePaused) {
        canLook = !gamePaused;
    }
}