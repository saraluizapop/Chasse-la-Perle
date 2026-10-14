using UnityEngine;

public class WaterScroller : MonoBehaviour
{
    [Header("Vitesse du courant")]
    public float vitesseX = 0.05f;
    public float vitesseY = 0.08f;

    private Renderer rendu;
    private Vector2 offset;

    void Start()
    {
        rendu = GetComponent<Renderer>();
    }

    void Update()
    {
        if (rendu == null) return;

        // Fait défiler les coordonnées UV en fonction du temps
        offset.x += vitesseX * Time.deltaTime;
        offset.y += vitesseY * Time.deltaTime;

        // Applique le décalage sur la texture principale (URP ou Standard)
        rendu.material.mainTextureOffset = offset;
    }
}