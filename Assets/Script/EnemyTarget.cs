using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [Header("Points de vie")]
    public int vie = 1;

    [Header("Particules de disparition (Optionnel)")]
    public GameObject particulesMort;

    public void SubirDegats(int quantite)
    {
        vie -= quantite;
        if (vie <= 0)
        {
            Mourir();
        }
    }

    private void Mourir()
    {
        // 1. Avertit le gestionnaire de l'arène
        ArenaManager arena = Object.FindAnyObjectByType<ArenaManager>();
        if (arena != null)
        {
            arena.MonstreMort();
        }

        // 2. Particules d'explosion magique
        if (particulesMort != null)
        {
            Instantiate(particulesMort, transform.position, Quaternion.identity);
        }

        // 3. Fait disparaître le monstre
        Destroy(gameObject);
    }
}