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

    void Mourir()
    {
        CastleGateManager gestionnaire = FindFirstObjectByType<CastleGateManager>();
        if (gestionnaire != null)
        {
            gestionnaire.EnnemiElimine();
        }

        if (particulesMort != null)
        {
            Instantiate(particulesMort, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}