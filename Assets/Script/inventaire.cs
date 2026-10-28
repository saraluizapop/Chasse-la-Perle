using UnityEngine;
using TMPro; // Nécessaire pour contrôler TextMeshPro

public class Inventaire : MonoBehaviour
{
    public int gemCount = 0;
    public TextMeshProUGUI compteurTexte; // Glisse ton Text (TMP) ici

    public void AddGem()
    {
        gemCount++;
        Debug.Log("Gemme ramassée ! Total : " + gemCount);

        // Met à jour l'affichage sur l'écran
        if (compteurTexte != null)
        {
            compteurTexte.text = gemCount + " / 3";
        }
    }
}