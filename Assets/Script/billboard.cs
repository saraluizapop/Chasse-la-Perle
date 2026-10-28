using System.Collections;
using UnityEngine;
using TMPro;

public class CatGuide : MonoBehaviour
{
    [Header("Éléments du HUD")]
    [Tooltip("Le panneau complet de dialogue dans le Canvas HUD")]
    public GameObject fenetreDialogueHUD;
    [Tooltip("Le composant TextMeshPro à l'intérieur du panneau")]
    public TextMeshProUGUI texteDialogueHUD;

    [Header("Paramètres d'affichage")]
    [Tooltip("Vitesse d'apparition des lettres")]
    public float vitesseEcriture = 0.03f;

    private Coroutine coroutineDialogue;

    void Start()
    {
        // Masque la boîte de dialogue au lancement du jeu
        if (fenetreDialogueHUD != null)
        {
            fenetreDialogueHUD.SetActive(false);
        }
    }

    // Fonction appelée par tes zones (CatTriggerHint) ou événements
    public void DonnerConseil(string message, float duree = 4.5f)
    {
        if (coroutineDialogue != null)
        {
            StopCoroutine(coroutineDialogue);
        }
        coroutineDialogue = StartCoroutine(AfficherDialogueRoutine(message, duree));
    }

    private IEnumerator AfficherDialogueRoutine(string message, float duree)
    {
        if (fenetreDialogueHUD == null || texteDialogueHUD == null) yield break;

        // Active le panneau HUD
        fenetreDialogueHUD.SetActive(true);
        texteDialogueHUD.text = "";

        // Effet machine à écrire
        foreach (char lettre in message.ToCharArray())
        {
            texteDialogueHUD.text += lettre;
            yield return new WaitForSeconds(vitesseEcriture);
        }

        // Temps de lecture
        yield return new WaitForSeconds(duree);

        // Masque le panneau une fois terminé
        fenetreDialogueHUD.SetActive(false);
    }
}