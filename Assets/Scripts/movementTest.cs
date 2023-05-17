using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementTest : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the player
    public float rotationSpeed = 180f; // Rotation speed of the camera

    private Rigidbody rb;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement = cameraTransform.TransformDirection(movement);
        movement.y = 0f;

        rb.velocity = movement * moveSpeed;

        // Camera rotation
        float rotateHorizontal = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0f, rotateHorizontal, 0f) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation);
    }
}
