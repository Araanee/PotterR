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

    [Header("UI Settings")]
    public Text powerUpText; // UI for Potions
    public AudioSource audioSource;
    public AudioClip potionClip; // Music for Invincibility Potion

    [Header("Game Over Settings")]
    public AudioClip mainMusicClip; // Assignez la musique principale ici
    public AudioClip gameOverClip; // Le son de game over

    // SYSTÈME DE NIVEAUX DE VITESSE SÉQUENTIEL
    public float[] speedLevels = { 10f, 15f, 20f, 25f };  // 4 niveaux de vitesse
    public float[] maxSpeedLevels = { 15f, 20f, 25f, 30f }; // maxSpeed correspondants
    public float speedChangeInterval = 5f;  // Intervalle entre chaque changement
    private float speedTimer = 0f;
    private int currentSpeedLevel = 0;

    PhotonView view;

    // Invincibility & Reverse
    private bool isInvincible = false;
    private bool reverseActive = false;
    private Coroutine reverseCoroutine;

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

                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || SwipeManager.swipeUp)
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

            if (Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown)
            {
                if (!controller.isGrounded)
                {
                    // Fast Fall
                    direction.y = -40f;
                    animator.SetBool("IsGrounded", true); // Prepare landing animation
                }
                else if (!isSliding)
                {
                    StartCoroutine(Slide());
                }
            }

            // CONTRÔLES AVEC INVERSION (POTION UNIQUEMENT)
            if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
            {
                if (reverseActive)
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
                if (reverseActive)
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

    private void Jump()
    {
        direction.y = jumpForce;
    }

    // ACTIVATION DE L'INVINCIBILITÉ (POTION)
    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityRoutine(duration));
    }

    // ACTIVATION DU REVERSE (POTION)
    public void ActivateReverse(float duration)
    {
        if (reverseCoroutine != null)
        {
            StopCoroutine(reverseCoroutine);
        }
        reverseCoroutine = StartCoroutine(ReverseCoroutine(duration));
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        if (powerUpText != null) powerUpText.text = "Felix Felicis !";

        // Play Potion Music
        if (audioSource != null && potionClip != null)
        {
            audioSource.clip = potionClip;
            audioSource.loop = true; // Loop if the clip is shorter than duration
            audioSource.Play();
        }

        StartCoroutine(Clignoter());
        Debug.Log("Invincibility Started!");

        yield return new WaitForSeconds(duration);

        isInvincible = false;
        if (powerUpText != null) powerUpText.text = "";

        // Stop Music
        if (audioSource != null && audioSource.clip == potionClip)
        {
            audioSource.Stop();
            audioSource.clip = mainMusicClip;
            audioSource.Play();

        }

        Debug.Log("Invincibility Ended!");
    }

    private IEnumerator ReverseCoroutine(float duration)
    {
        reverseActive = true;

        // Affichage UI
        if (powerUpText != null) powerUpText.text = "REVERSE MODE";

        Debug.Log("Reverse Mode Activated! Controls are inverted!");

        yield return new WaitForSeconds(duration);

        reverseActive = false;
        if (powerUpText != null) powerUpText.text = "";
        Debug.Log("Reverse Mode Deactivated! Controls are normal.");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("hit object");
        if (hit.transform.tag == "Obstacle")
        {
            Debug.Log("Hit Obstacle: " + hit.transform.name);

            if (isInvincible)
            {
                StartCoroutine(Clignoter());
            }
            else
            {
                
                PlayerManager.gameOver = true;

                // Trouver et arrêter la musique principale
                GameObject musicManager = GameObject.FindGameObjectWithTag("Music"); // ou FindWithTag
                if (musicManager != null)
                {
                    AudioSource musicSource = musicManager.GetComponent<AudioSource>();
                    if (musicSource != null && musicSource.isPlaying)
                    {
                        musicSource.Stop();
                    }
                }

                // Stop Music
                if (audioSource != null && audioSource.clip == potionClip)
                {
                    audioSource.Stop();
                    audioSource.loop = false;
                }

                // Jouer le son de game over
                if (gameOverClip != null)
                {
                    audioSource.PlayOneShot(gameOverClip);
                }

                // Hide UI
                if (powerUpText != null) powerUpText.text = "";
                
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
        // Blink effect
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < 5; i++)
        {
            foreach (var r in renderers) r.enabled = false;
            yield return new WaitForSeconds(0.1f);
            foreach (var r in renderers) r.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}