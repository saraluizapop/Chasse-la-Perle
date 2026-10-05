using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalAltar : MonoBehaviour
{
    [Header("Configuration")]
    public int gemmesRequises = 3;
    public string nomSceneFinale = "Niveau3"; // Nom exact de ta scène finale dans Unity

    [Header("Visuels du portail")]
    public GameObject vortexVisuel;     // L'effet de lumière ou le cylindre magique désactivé au départ
    public ParticleSystem activationParticles;

    [Header("Audio (optionnel)")]
    public AudioSource audioSource;
    public AudioClip soundActivation;

    private bool estActif = false;

    void Start()
    {
        // Le portail est éteint tant qu'on n'a pas les 3 gemmes
        if (vortexVisuel != null)
        {
            vortexVisuel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            // Cas 1 : Le portail est déjà actif -> téléporte vers la dernière île
            if (estActif)
            {
                SceneManager.LoadScene(nomSceneFinale);
                return;
            }

            // Cas 2 : Le portail est éteint -> vérifie l'inventaire
            Inventaire inventory = other.GetComponent<Inventaire>();
            if (inventory == null)
            {
                inventory = other.GetComponentInParent<Inventaire>();
            }

            if (inventory != null)
            {
                if (inventory.gemCount >= gemmesRequises)
                {
                    // Consomme les 3 gemmes et active le portail
                    inventory.gemCount -= gemmesRequises;
                    ActiverPortail();
                }
                else
                {
                    int manque = gemmesRequises - inventory.gemCount;
                    Debug.Log("Portail verrouillé ! Il manque " + manque + " gemme(s).");

                    // Fait parler le chat s'il est là
                    CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
                    if (guide != null)
                    {
                        guide.DonnerConseil("Le portail est inactif... Il nous manque encore " + manque + " gemme(s) !");
                    }
                }
            }
        }
    }

    private void ActiverPortail()
    {
        estActif = true;

        if (vortexVisuel != null)
        {
            vortexVisuel.SetActive(true);
        }

        if (activationParticles != null)
        {
            activationParticles.Play();
        }

        if (audioSource != null && soundActivation != null)
        {
            audioSource.PlayOneShot(soundActivation);
        }

        CatGuide guide = Object.FindAnyObjectByType<CatGuide>();
        if (guide != null)
        {
            guide.DonnerConseil("Le portail vers la dernière île est ouvert ! Entrons-y pour sauver la licorne !");
        }
    }
}