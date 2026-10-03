using UnityEngine;

public class CatFollower : MonoBehaviour
{
    [Header("Cible")]
    public Transform playerTarget;

    [Header("Position de suivi")]
    [Tooltip("Décalage par rapport au joueur (X: côté, Y: hauteur, Z: recul)")]
    public Vector3 offset = new Vector3(1.2f, 1.2f, -1.0f);
    public float followSpeed = 4f;
    public float rotationSpeed = 6f;

    [Header("Effet flottement magique (optionnel)")]
    public bool isFloating = true;
    public float bobbingSpeed = 2.5f;
    public float bobbingAmount = 0.15f;

    private Vector3 currentVelocity;

    void Start()
    {
        // Si la cible n'est pas assignée, cherche automatiquement le joueur
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }
    }

    void LateUpdate()
    {
        if (playerTarget == null) return;

        // Calcule la position désirée en fonction de l'orientation du joueur
        Vector3 targetPosition = playerTarget.TransformPoint(offset);

        // Ajoute un petit flottement vertical doux
        if (isFloating)
        {
            targetPosition.y += Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        }

        // Déplacement fluide vers le joueur
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / followSpeed);

        // Le chat regarde vers l'avant du joueur
        Vector3 lookDirection = playerTarget.forward;
        lookDirection.y = 0; // Garde la rotation horizontale droite
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
