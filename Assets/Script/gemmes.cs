using UnityEngine;

public class gemmes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
         
            Inventaire inventory = other.GetComponent<Inventaire>();
            if (inventory == null)
            {
                inventory = other.GetComponentInParent<Inventaire>();
            }

            if (inventory != null)
            {
                inventory.AddGem();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Le joueur a touché la gemme, mais le script 'Inventaire' est introuvable sur lui ou ses parents !");
            }
        }
    }
}
