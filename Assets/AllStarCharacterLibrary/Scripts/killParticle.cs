using UnityEngine;

public class killParticle : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Détruit l'objet dès que le ParticleSystem a fini d'émettre et est vide
        if (ps != null && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}