using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private void Start() {
        /// Sets the cursor lock state to locked and hides it.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        // We do this because of the way Unity handles rotation. Reference video: https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=9
        yRotation += mouseX;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamps the rotation so you can't look behind you.

        // Rotate the camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f); // Do it once to rotate the camera along both axes
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f); // To rotate the player along the y axis
    }
}
