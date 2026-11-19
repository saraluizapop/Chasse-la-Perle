using UnityEngine;

public class CatFollower : MonoBehaviour
{
    [Header("Cible (Joueur)")]
    public Transform playerTarget;

    [Header("Position au sol")]
    [Tooltip("Décalage horizontal derrière/à côté du joueur (X: côté, Z: recul)")]
    public Vector2 followOffset = new Vector2(-1.1f, -1.3f);
    public float smoothTimeWalk = 0.25f;
    public float smoothTimeSprint = 0.14f;
    public float rotationSpeed = 10f;
    [Tooltip("Distance minimale avant que le chat ne commence à marcher (évite les tremblements à l'arrêt)")]
    public float stopDistance = 0.25f;

    [Header("Détection du sol (Raycast)")]
    public LayerMask groundLayer = ~0;
    [Tooltip("Ajuste si les pattes du chat s'enfoncent ou flottent légèrement")]
    public float groundHeightOffset = 0f;
    public float raycastStartHeight = 3f;

    [Header("Animation")]
    public Animator animator;
    public string speedParameter = "Speed";
    public float sprintThreshold = 4.5f;

    private Vector3 lastPlayerPos;
    private Vector3 currentVelocity;
    private float smoothedY;
    private float smoothedAnimSpeed = 0f;
    private bool initializedY = false;

    void Start()
    {
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (playerTarget != null)
        {
            lastPlayerPos = playerTarget.position;
        }

        smoothedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (playerTarget == null) return;

        // 1. Calcule la vitesse réelle du joueur
        float dt = Mathf.Max(Time.deltaTime, 0.0001f);
        float playerSpeed = (playerTarget.position - lastPlayerPos).magnitude / dt;
        lastPlayerPos = playerTarget.position;

        bool isPlayerSprinting = playerSpeed > sprintThreshold;
        float currentSmoothTime = isPlayerSprinting ? smoothTimeSprint : smoothTimeWalk;

        // 2. Position horizontale cible à côté/derrière le joueur
        Vector3 desiredPos = playerTarget.position
                           + (playerTarget.right * followOffset.x)
                           + (playerTarget.forward * followOffset.y);

        // 3. Détection du sol sous la position cible (ignore le chat lui-même)
        float targetY = playerTarget.position.y + groundHeightOffset;
        Vector3 rayOrigin = new Vector3(desiredPos.x, playerTarget.position.y + raycastStartHeight, desiredPos.z);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastStartHeight * 3f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            // Vérifie que le rayon ne touche pas le chat ou un enfant du chat
            if (!hit.transform.IsChildOf(transform))
            {
                targetY = hit.point.y + groundHeightOffset;
            }
        }

        if (!initializedY)
        {
            smoothedY = targetY;
            initializedY = true;
        }
        else
        {
            // Lisse uniquement la hauteur Y pour éviter les sauts brusques sur les marches/pentes
            smoothedY = Mathf.Lerp(smoothedY, targetY, dt * 15f);
        }

        desiredPos.y = smoothedY;

        // 4. Vérifie la distance horizontale restante (Deadzone anti-tremblement)
        Vector3 flatCurrent = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatDesired = new Vector3(desiredPos.x, 0f, desiredPos.z);
        float horizontalDistance = Vector3.Distance(flatCurrent, flatDesired);

        Vector3 previousPos = transform.position;

        if (horizontalDistance > stopDistance)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, currentSmoothTime);
        }
        else
        {
            // Reste stable sur place tout en suivant doucement la hauteur du sol
            currentVelocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, smoothedY, transform.position.z);
        }

        // 5. Orientation fluide sans vibration
        Vector3 moveDelta = transform.position - previousPos;
        moveDelta.y = 0f;
        float catHorizontalSpeed = moveDelta.magnitude / dt;

        if (catHorizontalSpeed > 0.25f)
        {
            Quaternion moveRot = Quaternion.LookRotation(moveDelta.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRot, dt * rotationSpeed);
        }
        else
        {
            Vector3 lookDir = playerTarget.forward;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion idleRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, idleRot, dt * (rotationSpeed * 0.6f));
            }
        }

        // 6. Mise à jour fluide de l'Animator (0 = Idle, 2 = Walk, 6 = Sprint)
        if (animator != null)
        {
            float targetAnimValue = 0f;
            if (catHorizontalSpeed > 0.25f)
            {
                targetAnimValue = isPlayerSprinting ? 6f : 2f;
            }

            smoothedAnimSpeed = Mathf.Lerp(smoothedAnimSpeed, targetAnimValue, dt * 8f);
            animator.SetFloat(speedParameter, smoothedAnimSpeed);
        }
    }
}   