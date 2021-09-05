using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float fovDistanceMultiplier = 400f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> aggroTargets = new List<Transform>();

    private void Start()
    {
        StartCoroutine(FindAggroWithDelay(.2f));
    }

    IEnumerator FindAggroWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindAggroedTargets();
        }
    }

    void FindAggroedTargets()
    {
        aggroTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0;  i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            EnemyFieldOfView eFov = target.GetComponent<EnemyFieldOfView>();
            Vector3 dirFromTarget = (transform.position - target.position).normalized;
            if(Vector3.Angle(target.forward, dirFromTarget)< eFov.viewAngle / 2)
            {
                float distFromTarget = Vector3.Distance(target.position, transform.position);
                Debug.Log(distFromTarget);

                if (!Physics.Raycast(target.position, dirFromTarget, distFromTarget, obstacleMask))
                {
                    Debug.Log("Adding"+target.name);
                    eFov.viewAngle = fovDistanceMultiplier * 1 / distFromTarget;
                    aggroTargets.Add(target);
                }
            }
            if (aggroTargets.Contains(target))
            {
                target.GetComponent<Enemy>().Attack(true);
            }
            else
            {
                eFov.viewAngle = 90;
                target.GetComponent<Enemy>().Attack(false);
            }
        }
    }


}
