using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Points de vie (3 cœurs)")]
    public int maxVie = 3;
    public int vieActuelle;

    [Header("Icônes de vie sur le HUD")]
    [Tooltip("Glisse ici tes 3 images de cœurs dans l'ordre (Cœur 1, Cœur 2, Cœur 3)")]
    public Image[] coeursHUD;

    [Header("Camera Shake (Secousse d'écran)")]
    [Tooltip("Glisse ici la Main Camera (ou la caméra à secouer)")]
    public Transform cameraASecouer;
    public float dureeShake = 0.25f;
    public float intensiteShake = 0.25f;

    [Header("Invulnérabilité après un coup")]
    public float tempsInvulnerabilite = 1.2f;
    private bool estInvulnerable = false;

    [Header("Écran de défaite (Optionnel)")]
    public GameObject ecranDefaiteUI;

    void Start()
    {
        vieActuelle = maxVie;
        MettreAJourHUD();

        if (cameraASecouer == null && Camera.main != null)
        {
            cameraASecouer = Camera.main.transform;
        }
    }

    [System.Obsolete]
    public void SubirDegats(int quantite)
    {
        if (estInvulnerable || vieActuelle <= 0) return;

        vieActuelle = Mathf.Max(0, vieActuelle - quantite);
        MettreAJourHUD();

        // 1. Déclenche le tremblement d'écran
        if (cameraASecouer != null)
        {
            StartCoroutine(SecouerCamera());
        }

        // 2. Conseil du chat compagnon
        CatGuide guide = FindAnyObjectByType<CatGuide>();
        if (guide != null)
        {
            if (vieActuelle > 0)
                guide.DonnerConseil("Aïe ! Attention , il te reste " + vieActuelle + " vies!");
            else
                guide.DonnerConseil("Oh non ! Les échos ont eu raison de nous...");
        }

        // 3. Vérifie la défaite ou lance l'invulnérabilité temporaire
        if (vieActuelle <= 0)
        {
            Mourir();
        }
        else
        {
            StartCoroutine(InvulnerabiliteRoutine());
        }
    }

    private void MettreAJourHUD()
    {
        if (coeursHUD == null) return;

        for (int i = 0; i < coeursHUD.Length; i++)
        {
            if (coeursHUD[i] != null)
            {
                // Affiche le cœur tant que son index est inférieur à la vie restante
                coeursHUD[i].enabled = (i < vieActuelle);
            }
        }
    }

    private IEnumerator SecouerCamera()
    {
        Vector3 posOriginale = cameraASecouer.localPosition;
        float timer = 0f;

        while (timer < dureeShake)
        {
            timer += Time.deltaTime;
            float offsetX = Random.Range(-1f, 1f) * intensiteShake;
            float offsetY = Random.Range(-1f, 1f) * intensiteShake;

            cameraASecouer.localPosition = posOriginale + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }

        cameraASecouer.localPosition = posOriginale;
    }

    private IEnumerator InvulnerabiliteRoutine()
    {
        estInvulnerable = true;
        yield return new WaitForSeconds(tempsInvulnerabilite);
        estInvulnerable = false;
    }

    private void Mourir()
    {
        if (ecranDefaiteUI != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ecranDefaiteUI.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}