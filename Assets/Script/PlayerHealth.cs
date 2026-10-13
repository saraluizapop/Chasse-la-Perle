using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Points de vie")]
    public int maxVie = 3;
    public int vieActuelle;

    [Header("Invulnérabilité après coup")]
    public float tempsInvulnerabilite = 1.5f;
    private bool estInvulnerable = false;

    [Header("Écran de défaite (Optionnel)")]
    public GameObject ecranDefaiteUI;

    void Start()
    {
        vieActuelle = maxVie;
    }

    public void SubirDegats(int quantite)
    {
        if (estInvulnerable || vieActuelle <= 0) return;

        vieActuelle -= quantite;
        Debug.Log("Ouch ! Vie restante : " + vieActuelle);

        // Dialogue d'alerte du chat
        CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
        if (guide != null)
        {
            guide.DonnerConseil("Attention Lyra ! Éloigne-toi des monstres !");
        }

        if (vieActuelle <= 0)
        {
            Mourir();
        }
        else
        {
            StartCoroutine(ClignotementInvulnerabilite());
        }
    }

    private IEnumerator ClignotementInvulnerabilite()
    {
        estInvulnerable = true;
        yield return new WaitForSeconds(tempsInvulnerabilite);
        estInvulnerable = false;
    }

    private void Mourir()
    {
        Debug.Log("Défaite du joueur !");

        if (ecranDefaiteUI != null)
        {
            ecranDefaiteUI.SetActive(true);
        }
        else
        {
            // Recharge la scène actuelle si aucun écran de défaite n'est configuré
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}