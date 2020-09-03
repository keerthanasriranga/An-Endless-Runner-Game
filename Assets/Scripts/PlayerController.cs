using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1; //left=0, right=2
    public float laneDistance = 4; //distance between two lanes
    public float jumpForce;
    public float Gravity = -20;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;
        direction.z = forwardSpeed;
        direction.y += Gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }
      

        //Gather inputs on which lane we should be
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if(desiredLane==3)
                desiredLane=2;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if(desiredLane==-1)
                desiredLane=0;
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z*transform.forward +
                                 transform.position.y*transform.up;
 
        if(desiredLane==0)
        {
            targetPosition += Vector3.left*laneDistance;
        }

        else if(desiredLane==2)
        {
            targetPosition += Vector3.right*laneDistance;
        }

        transform.position = targetPosition;
        controller.center = controller.center;
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction*Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }

}
