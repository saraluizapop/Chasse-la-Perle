using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWander : MonoBehaviour
{
    [Header("Paramètres d'errance")]
    public float wanderRadius = 6f;      // Rayon adapté à ta dalle
    public float wanderTimer = 3f;       // Change de cible toutes les 3s

    [Header("Animation")]
    public Animator animator;
    public string walkParameter = "run"; // Assure-toi que c'est "run" ou "walking" selon ton Animator

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        timer = wanderTimer;
        ChoisirNouvelleDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            ChoisirNouvelleDestination();
            timer = 0;
        }

        // Vérifie si l'agent bouge réellement
        bool isMoving = agent.velocity.sqrMagnitude > 0.05f;

        if (animator != null)
        {
            animator.SetBool(walkParameter, isMoving);
        }
    }

    void ChoisirNouvelleDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        // Cherche un point valide sur le NavMesh à proximité
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Lapin : Nouvelle destination vers " + hit.position);
        }
        else
        {
            Debug.LogWarning("Lapin : Impossible de trouver un point sur le NavMesh ! Le lapin est-il bien sur la zone bleue ?");
        }
    }
}