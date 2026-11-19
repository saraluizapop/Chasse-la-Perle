using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerleVictoire : MonoBehaviour
{
    [Header("Mode de victoire")]
    [Tooltip("Coche pour charger une scène séparée, décoche pour afficher un panneau UI dans la même scène")]
    public bool chargerUneScene = true;
    public string nomSceneVictoire = "SceneVictoire";
    public GameObject ecranVictoireUI;

    [Header("Sécurité (Optionnel)")]
    [Tooltip("Glisse ici la cage de Perle pour empêcher la victoire tant que la cage est fermée")]
    public GameObject cageBloquante;

    [Header("Mise en scène")]
    public float delaiAvantVictoire = 1.5f;
    public AudioSource audioSource;
    public AudioClip sonVictoire;

    private bool victoireDeclenchee = false;

    private void OnTriggerEnter(Collider other)
    {
        if (victoireDeclenchee) return;

        // Bloque si la cage est encore active autour de Perle
        if (cageBloquante != null && cageBloquante.activeInHierarchy) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            victoireDeclenchee = true;
            StartCoroutine(DeclencherVictoire());
        }
    }

    private IEnumerator DeclencherVictoire()
    {
        // 1. Petit message final du chat compagnon
        CatGuide chat = FindFirstObjectByType<CatGuide>();
        if (chat != null)
        {
            chat.DonnerConseil("Perle ! Tu es saine et sauve !");
        }

        // 2. Son de victoire
        if (audioSource != null && sonVictoire != null)
        {
            audioSource.PlayOneShot(sonVictoire);
        }

        // 3. Laisse un court instant pour voir la réaction avant la transition
        yield return new WaitForSeconds(delaiAvantVictoire);

        // 4. Libère la souris pour les boutons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 5. Charge la scène de victoire ou affiche l'écran UI
        if (chargerUneScene)
        {
            SceneManager.LoadScene(nomSceneVictoire);
        }
        else if (ecranVictoireUI != null)
        {
            ecranVictoireUI.SetActive(true);
        }
    }
}