using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PortalAltar : MonoBehaviour
{
    [Header("Configuration")]
    public int gemmesRequises = 3;
    public string nomSceneFinale = "Niveau 3"; // Vérifie l'orthographe exacte dans Build Settings

    [Header("Visuels du portail")]
    public GameObject vortexVisuel;     // Visuel intérieur magique
    public ParticleSystem activationParticles;

    [Header("Audio (optionnel)")]
    public AudioSource audioSource;
    public AudioClip soundActivation;

    [Header("Effet de distorsion (Optionnel)")]
    public Volume globalVolume;
    public float dureeTransition = 1.5f;

    private bool estActif = false;
    private bool transitionEnCours = false;
    private ChromaticAberration chromatic;
    private LensDistortion lensDistortion;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (vortexVisuel != null)
        {
            vortexVisuel.SetActive(false);
        }

        // Tente de récupérer les composants du volume sans faire planter le jeu s'ils manquent
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out chromatic);
            globalVolume.profile.TryGet(out lensDistortion);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transitionEnCours) return;

        // Détection du joueur
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Le joueur entre en contact avec le portail !");

            // Si déjà actif, téléporte immédiatement
            if (estActif)
            {
                LancerTeleportation(other.gameObject);
                return;
            }

            // Vérification de l'inventaire
            Inventaire inventory = other.GetComponent<Inventaire>();
            if (inventory == null)
            {
                inventory = other.GetComponentInParent<Inventaire>();
            }

            if (inventory != null)
            {
                Debug.Log("Gemmes du joueur : " + inventory.gemCount + " / " + gemmesRequises);

                if (inventory.gemCount >= gemmesRequises)
                {
                    inventory.gemCount -= gemmesRequises;
                    ActiverPortail();
                    // Téléporte directement après activation !
                    LancerTeleportation(other.gameObject);
                }
                else
                {
                    int manque = gemmesRequises - inventory.gemCount;
                    Debug.LogWarning("Portail : Il manque encore " + manque + " gemme(s) !");

                    CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
                    if (guide != null)
                    {
                        guide.DonnerConseil("Il nous manque encore " + manque + " gemme(s) pour activer le portail !");
                    }
                }
            }
            else
            {
                Debug.LogError("Erreur : Aucun script 'Inventaire' trouvé sur le joueur ou ses parents !");
            }
        }
    }

    private void ActiverPortail()
    {
        estActif = true;

        if (vortexVisuel != null) vortexVisuel.SetActive(true);
        if (activationParticles != null) activationParticles.Play();
        if (audioSource != null && soundActivation != null) audioSource.PlayOneShot(soundActivation);

        CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
        if (guide != null)
        {
            guide.DonnerConseil("Le portail s'est ouvert ! En route vers la dernière île !");
        }
    }

    private void LancerTeleportation(GameObject joueur)
    {
        // Désactive les contrôles du joueur pour éviter qu'il ne bouge pendant la distorsion
        CharacterController cc = joueur.GetComponent<CharacterController>();
        if (cc == null) cc = joueur.GetComponentInParent<CharacterController>();
        if (cc != null) cc.enabled = false;

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        transitionEnCours = true;
        Debug.Log("Début de la transition vers " + nomSceneFinale);

        float temps = 0f;
        float fovDepart = (cam != null) ? cam.fieldOfView : 60f;

        while (temps < dureeTransition)
        {
            temps += Time.deltaTime;
            float t = temps / dureeTransition;

            if (chromatic != null) chromatic.intensity.value = Mathf.Lerp(0f, 1f, t);
            if (lensDistortion != null) lensDistortion.intensity.value = Mathf.Lerp(0f, -0.85f, t);
            if (cam != null) cam.fieldOfView = Mathf.Lerp(fovDepart, 130f, t);

            yield return null;
        }

        Debug.Log("Chargement de la scène : " + nomSceneFinale);
        SceneManager.LoadScene(nomSceneFinale);
    }
}