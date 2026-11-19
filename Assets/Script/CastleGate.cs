using UnityEngine;

public class CastleGateManager : MonoBehaviour
{
    [Header("Configuration")]
    public int ennemisRequis = 4;

    [Header("Accès au Château")]
    [Tooltip("La porte, grille ou barrière magique qui bloque l'entrée du château")]
    public GameObject porteChateau;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonOuverturePorte;
    [Range(0f, 1f)]
    public float volumeSon = 1f;

    private int ennemisTues = 0;

    public void EnnemiElimine()
    {
        ennemisTues++;
        Debug.Log("Ennemis vaincus : " + ennemisTues + " / " + ennemisRequis);

        CatGuide chat = FindFirstObjectByType<CatGuide>();
        if (chat != null && ennemisTues < ennemisRequis)
        {
            chat.DonnerConseil("Encore " + (ennemisRequis - ennemisTues) + " Échos avant le château !");
        }

        if (ennemisTues >= ennemisRequis)
        {
            OuvrirChateau();
        }
    }

    void OuvrirChateau()
    {
        if (porteChateau != null)
        {
            porteChateau.SetActive(false); // Ouvre le passage vers le château !
        }

        // Joue le bruit d'ouverture de la grille du château
        if (audioSource != null && sonOuverturePorte != null)
        {
            audioSource.PlayOneShot(sonOuverturePorte, volumeSon);
        }

        CatGuide chat = FindFirstObjectByType<CatGuide>();
        if (chat != null)
        {
            chat.DonnerConseil("La voie est libre ! Entrons dans le château sauver Perle !");
        }
    }
}