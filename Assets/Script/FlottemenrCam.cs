using UnityEngine;

public class MenuCameraFloat : MonoBehaviour
{
    public float vitesseFlottement = 0.5f;
    public float amplitudeX = 0.5f;
    public float amplitudeY = 0.2f;

    private Vector3 positionInitiale;

    void Start()
    {
        positionInitiale = transform.position;
    }

    void Update()
    {
        // Crée un mouvement très lent et doux en forme de 8
        float offsetX = Mathf.Sin(Time.time * vitesseFlottement) * amplitudeX;
        float offsetY = Mathf.Cos(Time.time * vitesseFlottement * 0.8f) * amplitudeY;

        transform.position = positionInitiale + new Vector3(offsetX, offsetY, 0);
    }
}