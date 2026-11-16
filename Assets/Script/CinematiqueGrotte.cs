using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject camCinematique;
    public GameObject joueur;
    public GameObject cameraJoueur;

    void OnEnable()
    {
        if (director == null) director = GetComponent<PlayableDirector>();
        // Écoute le signal de fin de la Timeline
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

        // 2. Active le personnage et la caméra de gameplay
        if (joueur != null) joueur.SetActive(true);
        if (cameraJoueur != null) cameraJoueur.SetActive(true);

        // 3. Désactive l'objet de cinématique
        gameObject.SetActive(false);
    }
}