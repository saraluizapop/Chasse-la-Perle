using UnityEngine;

public class CastleInteriorTrigger : MonoBehaviour
{
    public GameObject gestionnaireCage; // Glisse GestionnaireCage ici
    public GameObject[] gardesChateau;  // Glisse tes 3 ennemis du château ici

    private bool dejaDeclenche = false;

    private void OnTriggerEnter(Collider other)
    {
        if (dejaDeclenche) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            dejaDeclenche = true;

            // Active le compteur des 3 gardes
            if (gestionnaireCage != null) gestionnaireCage.SetActive(true);

            // Fait apparaître les 3 gardes autour de la cage
            foreach (GameObject garde in gardesChateau)
            {
                if (garde != null) garde.SetActive(true);
            }

            // Dialogue d'alerte du chat
            CatGuide chat = Object.FindAnyObjectByType<CatGuide>();
            if (chat != null)
            {
                chat.DonnerConseil("Attention Solange ! 3 gardes protègent la cage de Perle !");
            }
        }
    }
}