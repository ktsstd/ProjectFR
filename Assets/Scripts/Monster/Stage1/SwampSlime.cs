using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwampSlime : MonsterAI
{
    public override void Move()
    {
        if (target == null) return;
        isMoving = true;
        targetCollider = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = targetCollider.ClosestPoint(transform.position);
        float distance = Vector3.Distance(transform.position, targetPos); 
        if (distance <= attackRange)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("Run", false);
            if (attackTimer <= 0 && currentState != States.Attack)
            {
                currentState = States.Attack;
                isMoving = false;
                Attack();
            }
            else
            {
                Vector3 directionToTarget = target.position - transform.position;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 9f);
                }
            }
        }
        else
        {
            agent.SetDestination(target.position);
            animator.SetBool("Run", true);
        }
    }
}
