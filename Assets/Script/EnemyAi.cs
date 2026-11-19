using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Paramètres de poursuite et d'attaque")]
    public float porteeDetection = 12f;
    public float vitesseCourse = 3.5f;
    public float porteeAttaque = 1.6f;
    public int degats = 1;
    public float tempsEntreAttaques = 1.5f;

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
            Vector3 direction = (joueur.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (distance <= porteeAttaque)
            {
                if (animator != null) animator.SetBool("isWalking", false);

                if (Time.time >= prochainCoupTemps)
                {
                    AttaquerJoueur();
                    prochainCoupTemps = Time.time + tempsEntreAttaques;
                }
            }
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

        // Cherche PlayerHealth sur le joueur ou ses parents
        PlayerHealth sante = joueur.GetComponent<PlayerHealth>();
        if (sante == null) sante = joueur.GetComponentInParent<PlayerHealth>();
        if (sante == null) sante = joueur.GetComponentInChildren<PlayerHealth>();

        if (sante != null)
        {
            sante.SubirDegats(degats);
        }
    }
}