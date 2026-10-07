using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [Header("Objectif")]
    public int totalMonstres = 3;

    [Header("Éléments à débloquer")]
    public GameObject cageLicorne;       // La cage qui disparaît
    public GameObject ecranVictoireUI;   // Le canvas de fin de jeu

    private int monstresVaincus = 0;

    public void MonstreMort()
    {
        monstresVaincus++;
        Debug.Log("Monstres vaincus : " + monstresVaincus + " / " + totalMonstres);

        // Encouragement du chat
        CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
        if (guide != null && monstresVaincus < totalMonstres)
        {
            guide.DonnerConseil("Encore " + (totalMonstres - monstresVaincus) + " monstre(s) !");
        }

        // Victoire finale
        if (monstresVaincus >= totalMonstres)
        {
            LibererLicorne();
        }
    }

    private void LibererLicorne()
    {
        // Fait disparaître la cage
        if (cageLicorne != null)
        {
            cageLicorne.SetActive(false);
        }

        // Affiche l'écran de victoire
        if (ecranVictoireUI != null)
        {
            ecranVictoireUI.SetActive(true);
        }

        CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
        if (guide != null)
        {
            guide.DonnerConseil("Bravo Lyra ! Tu as vaincu les monstres et sauvé Perle !");
        }
    }
}