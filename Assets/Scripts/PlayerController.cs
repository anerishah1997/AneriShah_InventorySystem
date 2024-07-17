using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; 
    public float turnSpeed = 90f; 

    private CharacterController characterController;

    public float minX = -35f;
    public float maxX = 35f;
    public float minZ = -23f;
    public float maxZ = 40f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // get the value on X-axis
        float moveVertical = Input.GetAxis("Vertical"); // get the value on Y-axis

        // Rotate the player based on horizontal input
        transform.Rotate(0, moveHorizontal * turnSpeed * Time.deltaTime, 0);

        // Move the player forward/backward based on vertical input
        Vector3 move = transform.forward * moveVertical; // Move in the direction the player is facing
        characterController.Move(move * speed * Time.deltaTime);

        // Boundary check
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
        transform.position = clampedPosition;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
