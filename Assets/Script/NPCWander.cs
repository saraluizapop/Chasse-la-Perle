using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWander : MonoBehaviour
{
    [Header("Paramètres d'errance")]
    public float wanderRadius = 7f;      // Rayon de recherche de la destination
    public float wanderTimer = 4f;       // Temps d'attente avant de choisir un nouveau point

    [Header("Animation")]
    public Animator animator;
    public string walkParameter = "walking"; // Doit être identique au nom dans l'Animator

    private NavMeshAgent agent;
    private float timer;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Quand le timer expire, on cherche un nouveau point sur le NavMesh
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        // Met à jour l'animation : marche si l'agent avance et n'est pas arrivé
        if (animator != null)
        {
            bool isMoving = agent.hasPath && agent.remainingDistance > agent.stoppingDistance;
            animator.SetBool(walkParameter, isMoving);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}