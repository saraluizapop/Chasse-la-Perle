using UnityEngine;

public class gemmes :MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventaire inventory = other.GetComponent<Inventaire>();
            if (inventory != null)
            {
                inventory.AddGem();
                Destroy(gameObject); 
            }
        }
    }
}
