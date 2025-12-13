using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;
    private int desiredLane = 0; //0:middle 1:right -1:left
    public float laneDistance = 3; // distance btwn 2 lanes

    // PARAMÈTRES DE SAUT AMÉLIORÉS
    public float jumpForce = 17f;        // Augmentez cette valeur pour sauter plus haut (8-15 recommandé)
    public float Gravity = -30f;         // Gravité plus forte = saut plus rapide (-20 à -30 recommandé)
    public float fallMultiplier = 2.5f;  // Fait tomber plus vite que la montée (optionnel)

    public Animator animator;
    private bool isSliding = false;

    PhotonView view;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PlayerManager.gameOver && view.IsMine)
        {
            if (forwardSpeed < maxSpeed) //increase speed
                forwardSpeed += 0.1f * Time.deltaTime;

            direction.z = forwardSpeed;

            if (controller.isGrounded)
            {
                animator.SetBool("IsGrounded", true);
                direction.y = -1;

                if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp)
                {
                    Jump();
                    animator.SetBool("IsGrounded", false);
                }
            }
            else
            {
                // Applique une gravité plus forte quand on descend (optionnel pour un meilleur feeling)
                if (direction.y < 0)
                {
                    direction.y += Gravity * fallMultiplier * Time.deltaTime;
                }
                else
                {
                    direction.y += Gravity * Time.deltaTime;
                }
            }

            if ((Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown) && !isSliding)
            {
                StartCoroutine(Slide());
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
            {
                desiredLane++;
                if (desiredLane == 2) desiredLane = 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
            {
                desiredLane--;
                if (desiredLane == -2) desiredLane = -1;
            }

            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (desiredLane == -1)
            {
                targetPosition += Vector3.left * laneDistance;
            }
            else if (desiredLane == 1)
            {
                targetPosition += Vector3.right * laneDistance;
            }

            if (transform.position == targetPosition) return;
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 75 * Time.deltaTime;

            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce; // Retiré le * 1.5f pour plus de contrôle via l'inspector
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}