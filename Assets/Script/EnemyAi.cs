using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Paramètres de poursuite et d'attaque")]
    public float porteeDetection = 12f;
    public float vitesseCourse = 3.5f;
    public float porteeAttaque = 1.6f;
    public float tempsEntreAttaques = 1.5f;

    [Header("Défaite (Optionnel)")]
    [Tooltip("Glisse ton écran de défaite ici si tu veux qu'un coup fasse perdre le joueur")]
    public GameObject ecranDefaiteUI;

    private Transform joueur;
    private Animator animator;
    private float prochainCoupTemps = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        GameObject objJoueur = GameObject.FindGameObjectWithTag("Player");
        if (objJoueur != null)
        {
            joueur = objJoueur.transform;
        }
    }

    void Update()
    {
        if (joueur == null) return;

        float distance = Vector3.Distance(transform.position, joueur.position);

        if (distance <= porteeDetection)
        {
            // Pivote vers le joueur
            Vector3 direction = (joueur.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // À portée d'attaque : s'arrête et déclenche l'animation
            if (distance <= porteeAttaque)
            {
                if (animator != null) animator.SetBool("isWalking", false);

                if (Time.time >= prochainCoupTemps)
                {
                    AttaquerJoueur();
                    prochainCoupTemps = Time.time + tempsEntreAttaques;
                }
            }
            // Sinon : avance vers le joueur
            else
            {
                if (animator != null) animator.SetBool("isWalking", true);
                transform.position += transform.forward * vitesseCourse * Time.deltaTime;
            }
        }
        else
        {
            if (animator != null) animator.SetBool("isWalking", false);
        }
    }

    private void AttaquerJoueur()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Si tu as glissé un écran de défaite, il s'affiche quand l'ennemi frappe
        if (ecranDefaiteUI != null)
        {
            ecranDefaiteUI.SetActive(true);
        }
    }
}