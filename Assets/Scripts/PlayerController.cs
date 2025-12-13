using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public float jumpForce = 17f;
    public float Gravity = -30f;
    public float fallMultiplier = 2.5f;

    public Animator animator;
    private bool isSliding = false;
    private float gameTimer = 0f;
    private int lastTimeLeft = 0;

    [Header("Twist Settings")]
    public Text countdownText;
    public Text powerUpText; // New field for Potion UI
    public AudioSource audioSource;
    public AudioClip countdownClip;
    public AudioClip potionClip; // New field for Potion Music
    

    // SYSTÈME DE NIVEAUX DE VITESSE SÉQUENTIEL
    public float[] speedLevels = { 10f, 15f, 20f, 25f };  // 4 niveaux de vitesse
    public float[] maxSpeedLevels = { 15f, 20f, 25f, 30f }; // maxSpeed correspondants
    public float speedChangeInterval = 5f;  // Intervalle entre chaque changement
    private float speedTimer = 0f;
    private int currentSpeedLevel = 0;

    PhotonView view;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();

        // Commencer par la vitesse la plus faible (niveau 0)
        currentSpeedLevel = 0;
        ApplySpeedLevel(currentSpeedLevel);
        speedTimer = 0f;
    }

    void Update()
    {
        if (!PlayerManager.gameOver && view.IsMine)
        {
            // Mise à jour du timer de vitesse
            speedTimer += Time.deltaTime;

            // Changement de vitesse toutes les 5 secondes
            if (speedTimer >= speedChangeInterval)
            {
                ChangeSpeedSequential();
                speedTimer = 0f;
            }

            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.deltaTime;

            direction.z = forwardSpeed;

            if (controller.isGrounded)
            {
                animator.SetBool("IsGrounded", true);
                direction.y = -1;

                if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.Space)||SwipeManager.swipeUp)
                {
                    Jump();
                    animator.SetBool("IsGrounded", false);
                }
            }
            else
            {
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
            
            // Twist: Reverse controls after 20 seconds
            gameTimer += Time.deltaTime;
            
            // Countdown Logic (Starts at 15s, ends at 20s)
            if (gameTimer >= 15f && gameTimer < 20f)
            {
                int timeLeft = Mathf.CeilToInt(20f - gameTimer);
                if (timeLeft != lastTimeLeft)
                {
                    if (countdownText != null) countdownText.text = timeLeft.ToString();
                    if (audioSource != null && countdownClip != null) audioSource.PlayOneShot(countdownClip);
                    lastTimeLeft = timeLeft;
                }
            }
            else if (gameTimer >= 20f && lastTimeLeft != -1)
            {
                // Trigger once when we hit 20s
                if (countdownText != null) countdownText.text = "REVERSE!";
                lastTimeLeft = -1; // sentinel to say we passed 20s
                StartCoroutine(ClearText());
            }

            bool invertControls = gameTimer > 20f;

            if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
            {
                if (invertControls)
                {
                    desiredLane--;
                    if (desiredLane == -2) desiredLane = -1;
                }
                else
                {
                    desiredLane++;
                    if (desiredLane == 2) desiredLane = 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
            {
                if (invertControls)
                {
                    desiredLane++;
                    if (desiredLane == 2) desiredLane = 1;
                }
                else
                {
                    desiredLane--;
                    if (desiredLane == -2) desiredLane = -1;
                }
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

            // UNIFIED MOVEMENT LOGIC
            // Combine Forward/Vertical + Lateral movement
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 75 * Time.deltaTime; // Lateral speed
            
            // Limit lateral move to not over-shoot
            Vector3 lateralMove = Vector3.zero;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                lateralMove = moveDir;
            else
                lateralMove = diff;
                
            // Forward and Vertical Move (from direction)
            Vector3 forwardVerticalMove = direction * Time.deltaTime;
            
            // Final Move
            controller.Move(forwardVerticalMove + lateralMove);
        }
    }

    private void ChangeSpeedSequential()
    {
        // Passer au niveau suivant
        currentSpeedLevel = (currentSpeedLevel + 1) % speedLevels.Length;
        ApplySpeedLevel(currentSpeedLevel);

        Debug.Log("Vitesse niveau " + (currentSpeedLevel + 1) + " - Vitesse: " + forwardSpeed);
    }

    private void ApplySpeedLevel(int level)
    {
        forwardSpeed = speedLevels[level];
        maxSpeed = maxSpeedLevels[level];
    }

    // FixedUpdate removed to eliminate update desync




    private void Jump()
    {
        direction.y = jumpForce;
    }

    // Invincibility Logic
    private bool isInvincible = false;

    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityRoutine(duration));
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        if (powerUpText != null) powerUpText.text = "FELIX FELICIS!";
        
        // Play Potion Music
        if (audioSource != null && potionClip != null)
        {
            audioSource.clip = potionClip;
            audioSource.loop = true; // Loop if the clip is shorter than duration
            audioSource.Play();
        }

        StartCoroutine(Clignoter());
        Debug.Log("Invincibility Started!");
        // Optional: Visual effect (e.g., flash player, scale up)
        
        yield return new WaitForSeconds(duration);
        
        isInvincible = false;
        if (powerUpText != null) powerUpText.text = "";
        
        // Stop Music
        if (audioSource != null && audioSource.clip == potionClip)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        Debug.Log("Invincibility Ended!");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            // Debug Log for Collision
            Debug.Log("Hit Obstacle: " + hit.transform.name);
            
            if (isInvincible)
            {
                 // Ignore collision or destroy obstacle? 
                 // For now, just do nothing (pass through)
                StartCoroutine(Clignoter()); // Visual feedback could go here
            }
            else
            {
                 // Uncomment for real gameplay
                 PlayerManager.gameOver = true;
                 
                 // Hide Twist UI
                 if (countdownText != null) countdownText.text = "";
                 if (powerUpText != null) powerUpText.text = "";
                 if (audioSource != null) audioSource.Stop();
            }
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

    private IEnumerator Clignoter()
    {
        // Blink effect: Toggle visibility of the player mesh
        // This assumes the mesh is on a child object or the current object has a renderer
        // Simple implementation: disable/enable the SkinnedMeshRenderer if found, or just MeshRenderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        
        // Blink for 2 seconds (or however long we want visual feedback on hit)
        for (int i = 0; i < 5; i++)
        {
            foreach (var r in renderers) r.enabled = false;
            yield return new WaitForSeconds(0.1f);
            foreach (var r in renderers) r.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ClearText()
    {
        yield return new WaitForSeconds(2f);
        if (countdownText != null) countdownText.text = "";
    }
   
}

