using System.Collections;
using UnityEngine;

public class StatueExchange : MonoBehaviour
{
    [Header("Objet parent contenant les 6 nénuphars")]
    public GameObject missingLilypadsGroup;

    [Header("Nombre de gemmes requises")]
    public int requiredGems = 3;

    [Header("Paramètres d'apparition")]
    [Tooltip("Temps que met chaque nénuphar à grandir")]
    public float growDuration = 0.5f;

    [Tooltip("Délai d'attente entre l'apparition de chaque nénuphar")]
    public float delayBetweenLilypads = 0.25f;

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

                    // Lance l'animation progressive
                    StartCoroutine(ApparitionProgressive());

                    Debug.Log("Échange réussi : les nénuphars grandissent un par un !");
                }
                else
                {
                    int manque = requiredGems - inventory.gemCount;
                    Debug.Log("Il te manque encore " + manque + " gemme(s) pour la statue !");
                }
            }
        }
    }

    private IEnumerator ApparitionProgressive()
    {
        if (missingLilypadsGroup == null) yield break;

        // 1. Récupère tous les nénuphars enfants du groupe
        Transform parentTransform = missingLilypadsGroup.transform;
        int count = parentTransform.childCount;

        // Stocke leur taille d'origine et réduit-les à 0 au départ
        Vector3[] originalScales = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            Transform child = parentTransform.GetChild(i);
            originalScales[i] = child.localScale;
            child.localScale = Vector3.zero;
        }

        // 2. Active l'objet parent
        missingLilypadsGroup.SetActive(true);

        // 3. Fait grandir chaque nénuphar l'un après l'autre
        for (int i = 0; i < count; i++)
        {
            Transform child = parentTransform.GetChild(i);
            Vector3 targetScale = originalScales[i];

            float timer = 0f;
            while (timer < growDuration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / growDuration);

                // Effet d'interpolation fluide
                child.localScale = Vector3.Lerp(Vector3.zero, targetScale, Mathf.SmoothStep(0f, 1f, progress));
                yield return null;
            }

            child.localScale = targetScale;

            // Petit temps d'attente avant que le prochain ne pousse
            yield return new WaitForSeconds(delayBetweenLilypads);
        }
    }
}