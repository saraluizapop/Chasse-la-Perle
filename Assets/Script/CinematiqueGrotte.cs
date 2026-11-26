using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject camCinematique;
    public GameObject joueur;
    public GameObject cameraJoueur;
    public GameObject canvasHUD; // <-- Glisse ton Canvas HUD ici

    void OnEnable()
    {
        if (director == null) director = GetComponent<PlayableDirector>();

        // Cache le HUD pendant la cinématique
        if (canvasHUD != null) canvasHUD.SetActive(false);

        director.stopped += FinDeCinematique;
    }

    void OnDisable()
    {
        director.stopped -= FinDeCinematique;
    }

    void FinDeCinematique(PlayableDirector pd)
    {
        // 1. Coupe la caméra de cinématique
        if (camCinematique != null) camCinematique.SetActive(false);

        // 2. Réactive le personnage, sa caméra et le HUD
        if (joueur != null) joueur.SetActive(true);
        if (cameraJoueur != null) cameraJoueur.SetActive(true);
        if (canvasHUD != null) canvasHUD.SetActive(true);

        // 3. Verrouille le curseur pour le gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 4. Désactive l'objet de cinématique
        gameObject.SetActive(false);
    }
}