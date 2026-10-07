using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float vitesse = 20f;
    public float dureeDeVie = 3f;

    [Header("Dégâts")]
    public int degats = 1;

    void Start()
    {
        // Détruit le projectile après 3 secondes s'il ne touche rien
        Destroy(gameObject, dureeDeVie);
    }

    void Update()
    {
        // Avance droit devant lui
        transform.Translate(Vector3.forward * vitesse * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore le joueur pour ne pas exploser sur soi-même
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            return;
        }

        // Vérifie si l'objet touché est un monstre
        EnemyTarget monstre = other.GetComponent<EnemyTarget>();
        if (monstre == null)
        {
            monstre = other.GetComponentInParent<EnemyTarget>();
        }

        if (monstre != null)
        {
            monstre.SubirDegats(degats);
            Destroy(gameObject); // Détruit le projectile au contact du monstre
        }
        else if (!other.isTrigger)
        {
            // Détruit le projectile s'il touche un mur solide (qui n'est pas un trigger)
            Destroy(gameObject);
        }
    }
}