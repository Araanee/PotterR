using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CapsuleController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;

    public float forwardSpeed;
    public float maxSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (!PlayerManager.gameOver)
        {
            if (forwardSpeed < maxSpeed) //increase speed
                forwardSpeed +=  0.1f *  Time.deltaTime; 
                
            direction.z = forwardSpeed;

            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;


            if (transform.position == targetPosition) return;
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
                
            if (moveDir.sqrMagnitude < diff.sqrMagnitude) controller.Move(moveDir);
            else controller.Move(diff);
        
        }

    }

    private void FixedUpdate()
    {
        if (!PlayerManager.gameOver)
            controller.Move(direction * Time.fixedDeltaTime);
    }
   
}

