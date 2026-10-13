using UnityEngine;

public class MovingCliff : MonoBehaviour
{
    [Header("Points de déplacement")]
    [Tooltip("Décalage du mouvement (ex: X=0, Y=4, Z=0 pour monter/descendre)")]
    public Vector3 directionDeplacement = new Vector3(0, 4f, 0);

    [Header("Vitesse et Pause")]
    public float vitesse = 2f;
    public float tempsPause = 1f;

    private Vector3 pointDepart;
    private Vector3 pointArrivee;
    private bool versArrivee = true;
    private float tempsAttente = 0f;

    void Start()
    {
        pointDepart = transform.position;
        pointArrivee = pointDepart + directionDeplacement;
    }

    void Update()
    {
        if (tempsAttente > 0)
        {
            tempsAttente -= Time.deltaTime;
            return;
        }

        Vector3 cible = versArrivee ? pointArrivee : pointDepart;
        transform.position = Vector3.MoveTowards(transform.position, cible, vitesse * Time.deltaTime);

        // Quand la falaise atteint la destination
        if (Vector3.Distance(transform.position, cible) < 0.01f)
        {
            versArrivee = !versArrivee;
            tempsAttente = tempsPause;
        }
    }
}