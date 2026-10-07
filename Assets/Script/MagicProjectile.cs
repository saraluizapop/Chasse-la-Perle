using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [Header("Paramètres")]
    public float vitesse = 25f;
    public float dureeDeVie = 4f;
    public int degats = 1;

    void Start()
    {
        // Détruit le projectile après 4 secondes pour éviter qu'ils s'accumulent
        Destroy(gameObject, dureeDeVie);
    }

    void Update()
    {
        // Fait avancer le projectile droit devant lui
        transform.position += transform.forward * vitesse * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore le joueur et ses sous-objets pour ne pas se bloquer tout seul
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
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // Détruit le projectile au contact d'un mur ou du sol solide
            Destroy(gameObject);
        }
    }
}