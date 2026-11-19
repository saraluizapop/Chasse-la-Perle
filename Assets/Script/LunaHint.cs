using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CatGuide : MonoBehaviour
{
    [Header("Éléments du HUD")]
    [Tooltip("Le panneau complet de dialogue dans le Canvas HUD")]
    public GameObject fenetreDialogueHUD;
    [Tooltip("Le composant TextMeshPro à l'intérieur du panneau")]
    public TextMeshProUGUI texteDialogueHUD;

    [Header("Messages au démarrage (sans triggers)")]
    public bool jouerAuDemarrage = true;
    [TextArea(2, 4)]
    public string[] messagesIntro;

    [Header("Paramètres d'affichage")]
    [Tooltip("Vitesse d'apparition des lettres")]
    public float vitesseEcriture = 0.03f;
    [Tooltip("Temps d'attente automatique avant de passer au message suivant")]
    public float dureeLectureAuto = 5f;

    [Header("Touche pour passer / continuer")]
    public Key touchePasser = Key.E;

    private Coroutine coroutineDialogue;
    private bool dialogueActif = false;
    private bool passerDemande = false;

    void Start()
    {
        if (fenetreDialogueHUD == null || texteDialogueHUD == null)
        {
            Debug.LogError("ATTENTION : Il manque FenetreDialogueHUD ou TexteDialogueHUD sur " + gameObject.name);
            return;
        }

        fenetreDialogueHUD.SetActive(false);

        if (jouerAuDemarrage && messagesIntro != null && messagesIntro.Length > 0)
        {
            DonnerSequenceConseils(messagesIntro);
        }
    }

    void Update()
    {
        if (dialogueActif && Keyboard.current != null && Keyboard.current[touchePasser].wasPressedThisFrame)
        {
            PasserDialogue();
        }
    }

    public void PasserDialogue()
    {
        if (!dialogueActif) return;
        passerDemande = true;
    }

    public void DonnerConseil(string message, float duree = 4.5f)
    {
        DonnerSequenceConseils(new string[] { message }, duree);
    }

    public void DonnerSequenceConseils(string[] messages, float dureeParMessage = -1f)
    {
        if (messages == null || messages.Length == 0) return;

        if (dureeParMessage <= 0f)
        {
            dureeParMessage = dureeLectureAuto;
        }

        if (coroutineDialogue != null)
        {
            StopCoroutine(coroutineDialogue);
        }
        coroutineDialogue = StartCoroutine(AfficherSequenceRoutine(messages, dureeParMessage));
    }

    private IEnumerator AfficherSequenceRoutine(string[] messages, float dureeParMessage)
    {
        if (fenetreDialogueHUD == null || texteDialogueHUD == null) yield break;

        dialogueActif = true;
        fenetreDialogueHUD.SetActive(true);

        for (int i = 0; i < messages.Length; i++)
        {
            string messageActuel = messages[i];
            texteDialogueHUD.text = "";
            passerDemande = false;

            foreach (char lettre in messageActuel.ToCharArray())
            {
                if (passerDemande)
                {
                    texteDialogueHUD.text = messageActuel;
                    passerDemande = false;
                    break;
                }

                texteDialogueHUD.text += lettre;
                yield return new WaitForSeconds(vitesseEcriture);
            }

            yield return null;

            float timer = 0f;
            while (timer < dureeParMessage)
            {
                if (passerDemande)
                {
                    passerDemande = false;
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }

        fenetreDialogueHUD.SetActive(false);
        dialogueActif = false;
        passerDemande = false;
    }
}