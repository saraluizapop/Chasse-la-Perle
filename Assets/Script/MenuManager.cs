using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Nom de la scène de jeu")]
    public string sceneACharger = "Niveau 1"; // À vérifier avec le nom exact de ta scène

    [Header("Panneaux")]
    public GameObject panelControles;
    public GameObject panelCredits;

    public void LancerJeu()
    {
        SceneManager.LoadScene(sceneACharger);
    }

    public void OuvrirControles()
    {
        panelControles.SetActive(true);
        panelCredits.SetActive(false);
    }

    public void OuvrirCredits()
    {
        panelCredits.SetActive(true);
        panelControles.SetActive(false);
    }

    public void FermerPanneaux()
    {
        panelControles.SetActive(false);
        panelCredits.SetActive(false);
    }
}