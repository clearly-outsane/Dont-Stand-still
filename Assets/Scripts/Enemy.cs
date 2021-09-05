using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float maxHealth = 100f;
    float currentHealth;
    NavMeshAgent agent;
    Transform target;
    public bool isChasing = false;
    public float attackDamage = 20f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.1f;
    [SerializeField] private Transform points;
    [SerializeField] private int destPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        target = RefManager.GetPlayer.transform;

        destPoint = Mathf.RoundToInt(Random.Range(0f, points.childCount));
        GotoNextPoint();
    }

    public void Attack(bool chase)
    {
        isChasing = chase;
        float distance = Vector3.Distance(target.position, transform.position);

        if (isChasing)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                //Attack player
                FaceTarget();
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        
    }

    void PerformAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, GetComponent<FieldOfView>().targetMask);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().Damage(attackDamage);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*3);
    }

    public void Damage(float dmg)
    {
        currentHealth -= dmg;
        //Play dmg animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Die animation
        Debug.Log("Dead");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.childCount == 0)
            return;

        destPoint = destPoint % points.childCount;
        // Set the agent to go to the currently selected destination.
        agent.destination = points.GetChild(destPoint).position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = Mathf.RoundToInt(Random.Range(0f, points.childCount));
        destPoint = (destPoint + 1) % points.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(agent.remainingDistance + " " + name);
        if (!agent.pathPending && agent.remainingDistance < 10f && !isChasing)
        {
            
            GotoNextPoint();
        }
            
    }
}
