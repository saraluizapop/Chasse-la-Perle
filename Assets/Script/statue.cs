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

    [Header("Son d'apparition")]
    public AudioSource audioSource;
    public AudioClip sonApparitionNenuphar;
    [Range(0f, 1f)]
    public float volumeSon = 0.8f;

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

                    // Lance l'animation progressive avec son
                    StartCoroutine(ApparitionProgressive());
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

        Transform parentTransform = missingLilypadsGroup.transform;
        int count = parentTransform.childCount;

        Vector3[] originalScales = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            Transform child = parentTransform.GetChild(i);
            originalScales[i] = child.localScale;
            child.localScale = Vector3.zero;
        }

        missingLilypadsGroup.SetActive(true);

        for (int i = 0; i < count; i++)
        {
            Transform child = parentTransform.GetChild(i);
            Vector3 targetScale = originalScales[i];

            // Joue le bruit juste au moment où ce nénuphar commence à apparaître
            if (audioSource != null && sonApparitionNenuphar != null)
            {
                // Optionnel : fait monter légèrement le pitch (la note) à chaque nénuphar (1.0 -> 1.3)
                audioSource.pitch = 1.0f + (i * 0.06f);
                audioSource.PlayOneShot(sonApparitionNenuphar, volumeSon);
            }

            float timer = 0f;
            while (timer < growDuration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / growDuration);
                child.localScale = Vector3.Lerp(Vector3.zero, targetScale, Mathf.SmoothStep(0f, 1f, progress));
                yield return null;
            }

            child.localScale = targetScale;

            yield return new WaitForSeconds(delayBetweenLilypads);
        }

        // Remet le pitch normal à la fin
        if (audioSource != null) audioSource.pitch = 1.0f;
    }
}