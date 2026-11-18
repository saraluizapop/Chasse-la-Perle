using UnityEngine;

public class gemmes : MonoBehaviour
{
    [Header("Son de collecte (Rétroaction)")]
    public AudioClip sonCollecte;
    [Range(0f, 1f)]
    public float volume = 0.8f;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet qui touche possède le tag Player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            // Cherche le script Inventaire sur l'objet ou sur ses parents[cite: 1]
            Inventaire inventory = other.GetComponent<Inventaire>();
            if (inventory == null)
            {
                inventory = other.GetComponentInParent<Inventaire>();
            }

            if (inventory != null)
            {
                inventory.AddGem();

                // Joue le bruit de rétroaction à l'endroit exact de la gemme
                if (sonCollecte != null)
                {
                    AudioSource.PlayClipAtPoint(sonCollecte, transform.position, volume);
                }

                Destroy(gameObject);
            }
        }
    }
}