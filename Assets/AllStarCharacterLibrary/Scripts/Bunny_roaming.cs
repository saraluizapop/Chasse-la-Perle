using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class BunnyRoam : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 6f;
    public float roamRadius = 8f;

    [Header("Idle Settings")]
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 startingPosition;
    private Vector3 targetDestination;
    private bool isWalking = false;

    // Gravity
    private float verticalVelocity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        startingPosition = transform.position;

        StartCoroutine(RoamRoutine());
    }

    void Update()
    {
        if (isWalking)
        {
            Vector3 direction = (targetDestination - transform.position);
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.05f)
            {
                // Smoothly rotate towards the target point
                Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);

                // Move forward with gravity
                Vector3 moveVelocity = transform.forward * moveSpeed;
                moveVelocity.y = controller.isGrounded ? -1f : verticalVelocity;

                controller.Move(moveVelocity * Time.deltaTime);

                // Update animator parameter (Course 09)
                animator.SetFloat("Speed", moveSpeed);
            }
            else
            {
                // Destination reached
                isWalking = false;
                animator.SetFloat("Speed", 0f);
            }
        }
        else
        {
            // Apply slight downward grounding force when stopped
            if (!controller.isGrounded)
            {
                controller.Move(new Vector3(0, verticalVelocity * Time.deltaTime, 0));
            }
            animator.SetFloat("Speed", 0f);
        }
    }

    private IEnumerator RoamRoutine()
    {
        while (true)
        {
            // 1. Wait/Idle for a random amount of time
            isWalking = false;
            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);

            // 2. Pick a random nearby spot within the roam radius
            Vector2 randomCircle = Random.insideUnitCircle * roamRadius;
            targetDestination = new Vector3(
                startingPosition.x + randomCircle.x,
                transform.position.y,
                startingPosition.z + randomCircle.y
            );

            // 3. Walk towards that spot (timeout after 6 seconds if stuck)
            isWalking = true;
            float walkTimer = 0f;
            while (isWalking && walkTimer < 6f)
            {
                walkTimer += Time.deltaTime;
                yield return null;
            }

            isWalking = false;
        }
    }

    // Displays the roaming zone circle in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = Application.isPlaying ? startingPosition : transform.position;
        Gizmos.DrawWireSphere(center, roamRadius);
    }
}