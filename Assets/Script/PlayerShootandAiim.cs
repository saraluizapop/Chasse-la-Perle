using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootandAiim : MonoBehaviour
{
    [Header("Caméra & Tir")]
    public Transform playerCameraRoot;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float tempsEntreTirs = 0.25f;

    private float prochainTir = 0f;

    void Start()
    {
        if (playerCameraRoot == null && Camera.main != null)
            playerCameraRoot = Camera.main.transform;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Time.time >= prochainTir)
        {
            prochainTir = Time.time + tempsEntreTirs;
            Tirer();
        }
    }

    void LateUpdate()
    {
        if (playerCameraRoot == null) return;
        Quaternion camRot = playerCameraRoot.rotation;
        Quaternion targetRot = Quaternion.Euler(0, camRot.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 25f);
        playerCameraRoot.rotation = camRot;
    }

   void Tirer()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("ERREUR : La case Projectile Prefab est vide dans l'Inspecteur !");
            return;
        }

        // Garde UNIQUEMENT la direction horizontale (gauche/droite) de la caméra pour tirer bien droit devant !
        float angleHorizontal = (playerCameraRoot != null) ? playerCameraRoot.eulerAngles.y : transform.eulerAngles.y;
        Quaternion direction = Quaternion.Euler(0, angleHorizontal, 0);

        Vector3 depart = (firePoint != null) ? firePoint.position : transform.position + Vector3.up * 1.2f;
        Vector3 spawnPos = depart + (direction * Vector3.forward * 0.8f);

        Instantiate(projectilePrefab, spawnPos, direction);
    }
    
}