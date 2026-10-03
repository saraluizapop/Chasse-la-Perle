using UnityEngine;

public class StatueExchange : MonoBehaviour
{
    [Header("Objet parent contenant les 6 nénuphars")]
    public GameObject missingLilypadsGroup;

    [Header("Nombre de gemmes requises")]
    public int requiredGems = 3;

    private bool hasExchanged = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasExchanged) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            Inventaire inventory = other.GetComponent<Inventaire>();
            if (inventory == null)
            {
                inventory = other.GetComponentInParent<Inventaire>();
            }

            if (inventory != null)
            {
                if (inventory.gemCount >= requiredGems)
                {
                    inventory.gemCount -= requiredGems;
                    hasExchanged = true;

                    if (missingLilypadsGroup != null)
                    {
                        missingLilypadsGroup.SetActive(true);
                    }

                    Debug.Log("Échange réussi : le pont de nénuphars apparaît !");
                }
                else
                {
                    int manque = requiredGems - inventory.gemCount;
                    Debug.Log("Il te manque encore " + manque + " gemme(s) pour la statue !");
                }
            }
        }
    }
}