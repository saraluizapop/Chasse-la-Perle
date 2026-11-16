using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float vitesse = 25f;
    public float dureeDeVie = 3f;
    public int degats = 1;

    void Start()
    {
        Destroy(gameObject, dureeDeVie);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * vitesse * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore le joueur et les zones invisibles (triggers)
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player") || other.isTrigger)
            return;

        // Cherche le script de l'ennemi
        EnemyTarget ennemi = other.GetComponent<EnemyTarget>();
        if (ennemi == null) ennemi = other.GetComponentInParent<EnemyTarget>();

        if (ennemi != null)
        {
            ennemi.SubirDegats(degats);
        }

        // Détruit le projectile dès qu'il touche un ennemi ou un mur
        Destroy(gameObject);
    }
}