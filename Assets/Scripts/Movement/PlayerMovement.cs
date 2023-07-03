using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Other")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update() {
        // Check if player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // Apply drag if player is grounded
        if (grounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0f;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jumping
        if (Input.GetKey(jumpKey) && readyToJump && grounded) { // If the player presses the jump key and is ready to jump
            readyToJump = false;
            
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); // Invoke the ResetJump function after the jumpCooldown time
        }
    }

    private void MovePlayer(){
        // Calculate move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (grounded) 
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); // The tutorial said to add 10f to the moveSpeed to make it faster (I guess it is to make the variable scale more manageable)
        
        // In air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; // Normalize the velocity to get the direction, then multiply it by the moveSpeed to get the limited velocity
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // Set the velocity to the limited velocity
        }
    }

    private void Jump(){
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Add jump force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(){
        readyToJump = true;
    }
}
