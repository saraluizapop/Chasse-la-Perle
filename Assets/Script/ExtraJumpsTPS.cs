using UnityEngine;
using StarterAssets;

[DefaultExecutionOrder(-100)] // S'exécute avant le ThirdPersonController
public class ExtraJumpsTPS : MonoBehaviour
{
    public float[] hauteursSauts = { 4f, 3f };

    int prochainSaut;
    bool sautPrecedent;
    ThirdPersonController controleur;
    StarterAssetsInputs input;

    void Awake()
    {
        // Try finding on the same GameObject, then search parent/children if nested
        controleur = GetComponent<ThirdPersonController>() ?? GetComponentInParent<ThirdPersonController>() ?? GetComponentInChildren<ThirdPersonController>();
        input = GetComponent<StarterAssetsInputs>() ?? GetComponentInParent<StarterAssetsInputs>() ?? GetComponentInChildren<StarterAssetsInputs>();
    }

    void Start()
    {
        if (controleur != null)
        {
            controleur.JumpTimeout = 0;
        }
        else
        {
            Debug.LogError("ExtraJumpsTPS: ThirdPersonController could not be found!", this);
        }

        if (input == null)
        {
            Debug.LogError("ExtraJumpsTPS: StarterAssetsInputs could not be found!", this);
        }
    }

    void Update()
    {
        // Prevent NullReferenceException if components are not yet available
        if (input == null || controleur == null) return;
        if (hauteursSauts.Length == 0) return;

        bool nouvelAppui = input.jump && !sautPrecedent;
        sautPrecedent = input.jump;

        if (controleur.Grounded)
        {
            prochainSaut = 1; // Index du prochain saut dans hauteursSauts (0 = saut du sol)
            controleur.JumpHeight = hauteursSauts[0];
        }
        else if (nouvelAppui && prochainSaut < hauteursSauts.Length)
        {
            controleur.JumpHeight = hauteursSauts[prochainSaut];
            prochainSaut++;
            controleur.Grounded = true;
        }
    }

    // Ajoute un saut de plus (à appeler par ex. depuis un Invoke Events du Collider Event System)
    public void AjouterSaut(float hauteur)
    {
        System.Array.Resize(ref hauteursSauts, hauteursSauts.Length + 1);
        hauteursSauts[hauteursSauts.Length - 1] = hauteur;
    }
}