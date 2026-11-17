using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public int vie = 1;
    public GameObject particulesMort;

    public void SubirDegats(int degats)
    {
        vie -= degats;
        if (vie <= 0)
        {
            Mourir();
        }
    }

    private void Mourir()
    {
        // 1. Préviens le gestionnaire de la porte extérieure (s'il est actif)
        CastleGateManager gateManager = Object.FindAnyObjectByType<CastleGateManager>();
        if (gateManager != null && gateManager.gameObject.activeInHierarchy)
        {
            gateManager.EnnemiElimine();
        }

        // 2. Préviens le gestionnaire de la cage intérieure (s'il est actif)
        ArenaManager arena = Object.FindAnyObjectByType<ArenaManager>();
        if (arena != null && arena.gameObject.activeInHierarchy)
        {
            arena.MonstreMort();
        }

        // 3. Particules de mort
        if (particulesMort != null)
        {
            Instantiate(particulesMort, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}