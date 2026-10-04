using UnityEngine;

public class CatTriggerHint : MonoBehaviour
{
    [Header("Message du chat")]
    [TextArea(2, 4)]
    public string message = "Regarde bien autour de toi pour trouver les 3 gemmes !";
    public float dureeAffichage = 4.5f;

    [Header("Options")]
    public bool neDireQuUneSeuleFois = true;

    private bool dejaDeclenche = false;

    private void OnTriggerEnter(Collider other)
    {
        if (dejaDeclenche && neDireQuUneSeuleFois) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            // Cherche le chat dans la scène
            CatGuide guide = FindAnyObjectByType<CatGuide>();
            if (guide != null)
            {
                guide.DonnerConseil(message, dureeAffichage);
                dejaDeclenche = true;
            }
        }
    }
}
