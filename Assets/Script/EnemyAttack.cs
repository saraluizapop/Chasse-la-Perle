
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Paramètres d'attaque")]
    public float porteeDetection = 10f; // Distance à partir de laquelle il repère le joueur
    public float vitesseCourse = 3.5f;
    public float porteeAttaque = 1.2f;  // Distance pour toucher
    public int degats = 1;
    public float tempsEntreAttaques = 1.5f;

    private Transform joueur;
    private float prochainCoupTemps = 0f;

    void Start()
    {
        // Trouve automatiquement le joueur grâce à son Tag
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

        // 1. Si le joueur est à portée de détection
        if (distance <= porteeDetection)
        {
            // Regarde en direction du joueur (sans pencher la tête en haut/bas)
            Vector3 direction = (joueur.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // 2. Si le monstre est assez proche pour attaquer
            if (distance <= porteeAttaque)
            {
                if (Time.time >= prochainCoupTemps)
                {
                    AttaquerJoueur();
                    prochainCoupTemps = Time.time + tempsEntreAttaques;
                }
            }
            else
            {
                // 3. Sinon, il avance vers le joueur
                transform.position += transform.forward * vitesseCourse * Time.deltaTime;
            }
        }
    }

    private void AttaquerJoueur()
    {
        PlayerHealth sante = joueur.GetComponent<PlayerHealth>();
        if (sante == null)
        {
            sante = joueur.GetComponentInParent<PlayerHealth>();
        }

        if (sante != null)
        {
            sante.SubirDegats(degats);
        }
    }

    // Affiche la zone de détection en fil de fer jaune dans la vue Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, porteeDetection);
    }
}