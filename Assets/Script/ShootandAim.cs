using UnityEngine;
using UnityEngine.InputSystem; // Indispensable pour le New Input System

public class PlayerShootAndAim : MonoBehaviour
{
    [Header("Caméra & Orientation")]
    [Tooltip("Objet pivot de la caméra (ex: PlayerCameraRoot ou Main Camera)")]
    public Transform playerCameraRoot;
    [Tooltip("Vitesse de rotation pour aligner le joueur")]
    public float rotationSpeed = 25f;

    [Header("Tir Magique")]
    [Tooltip("Le Prefab du projectile magique")]
    public GameObject projectilePrefab;
    [Tooltip("Point précis devant la baguette ou la main (Optionnel)")]
    public Transform firePoint;

    [Header("Audio (Optionnel)")]
    public AudioSource audioSource;
    public AudioClip sonTir;

    void Start()
    {
        // Si le champ n'est pas assigné, recherche automatiquement la caméra principale
        if (playerCameraRoot == null && Camera.main != null)
        {
            playerCameraRoot = Camera.main.transform;
        }
    }

    void Update()
    {
        // Détection du clic gauche avec le New Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Tirer();
        }
    }

    void LateUpdate()
    {
        if (playerCameraRoot == null) return;

        // 1. Mémorise l'orientation actuelle du pivot caméra
        Quaternion cameraRotation = playerCameraRoot.rotation;

        // 2. Aligne horizontalement le corps du joueur avec la caméra (Yaw)
        Quaternion targetRotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 3. Rétablit l'orientation du pivot pour conserver le regard vertical
        playerCameraRoot.rotation = cameraRotation;
    }

    private void Tirer()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlayerShootAndAim : Aucun prefab de projectile assigné !");
            return;
        }

        // Calcule le point d'apparition
        Vector3 spawnPos = (firePoint != null)
            ? firePoint.position
            : transform.position + transform.forward * 1f + Vector3.up * 1f;

        // La rotation du tir prend toute l'orientation de la caméra (haut, bas, avant)
        Quaternion spawnRot = (playerCameraRoot != null) ? playerCameraRoot.rotation : transform.rotation;

        // Instancie le projectile magique
        Instantiate(projectilePrefab, spawnPos, spawnRot);

        // Joue le son de tir s'il est configuré
        if (audioSource != null && sonTir != null)
        {
            audioSource.PlayOneShot(sonTir);
        }
    }
}