using System.Collections;
using UnityEngine;

public class StatueExchange : MonoBehaviour
{
    [Header("Configuration de l'échange")]
    public int gemmesRequises = 3;
    [Tooltip("Glisse ici l'objet parent Pont_Nenuphars contenant tes 6 nénuphars")]
    public GameObject pontNenuphars;

    [Header("Animation des nénuphars")]
    public float dureeApparition = 0.6f;
    public float delaiEntreNenuphars = 0.25f;

    [Header("Dialogue du chat après l'échange")]
    [TextArea(2, 4)]
    public string[] messagesStatueActivee = new string[]
    {
        "Incroyable ! La statue a accepté les 3 gemmes !",
        "Regarde, les nénuphars ont poussé ! Vite, traversons vers le Jardin secret !"
    };

    [Header("Audio (Optionnel)")]
    public AudioSource audioSource;
    public AudioClip sonActivationStatue;
    public AudioClip sonNenuphar;

    private bool dejaActivee = false;

    void Start()
    {
        // Cache le groupe complet des 6 nénuphars au début
        if (pontNenuphars != null)
        {
            pontNenuphars.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dejaActivee) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            Inventaire inventaire = other.GetComponent<Inventaire>();
            if (inventaire == null) inventaire = other.GetComponentInParent<Inventaire>();

            if (inventaire != null)
            {
                if (inventaire.gemCount >= gemmesRequises)
                {
                    ActiverStatue(inventaire);
                }
                else
                {
                    CatGuide chat = FindFirstObjectByType<CatGuide>();
                    if (chat != null)
                    {
                        int manquantes = gemmesRequises - inventaire.gemCount;
                        chat.DonnerConseil("Il te manque encore " + manquantes + " gemme(s) pour réveiller la statue !");
                    }
                }
            }
        }
    }

    private void ActiverStatue(Inventaire inventaire)
    {
        dejaActivee = true;

        inventaire.gemCount -= gemmesRequises;
        if (inventaire.compteurTexte != null)
        {
            inventaire.compteurTexte.text = inventaire.gemCount + " / " + gemmesRequises;
        }

        if (audioSource != null && sonActivationStatue != null)
        {
            audioSource.PlayOneShot(sonActivationStatue);
        }

        StartCoroutine(FaireApparaitreGroupe());

        CatGuide chat = FindFirstObjectByType<CatGuide>();
        if (chat != null && messagesStatueActivee != null && messagesStatueActivee.Length > 0)
        {
            chat.DonnerSequenceConseils(messagesStatueActivee);
        }
    }

    private IEnumerator FaireApparaitreGroupe()
    {
        if (pontNenuphars == null) yield break;

        Transform parent = pontNenuphars.transform;
        int count = parent.childCount;
        Vector3[] taillesOriginales = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            Transform enfant = parent.GetChild(i);
            taillesOriginales[i] = enfant.localScale;
            enfant.localScale = Vector3.zero;
        }

        pontNenuphars.SetActive(true);

        for (int i = 0; i < count; i++)
        {
            Transform enfant = parent.GetChild(i);
            Vector3 cible = taillesOriginales[i];

            if (audioSource != null && sonNenuphar != null)
            {
                audioSource.PlayOneShot(sonNenuphar);
            }

            float timer = 0f;
            while (timer < dureeApparition)
            {
                timer += Time.deltaTime;
                float progression = Mathf.SmoothStep(0f, 1f, timer / dureeApparition);
                enfant.localScale = Vector3.Lerp(Vector3.zero, cible, progression);
                yield return null;
            }

            enfant.localScale = cible;
            yield return new WaitForSeconds(delaiEntreNenuphars);
        }
    }
}