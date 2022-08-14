using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChaseState : IState
{
    EnemyController enemy;

    Transform enemyTransform;

    public ChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.isWalk = true;
        enemyTransform = enemy.gameObject.transform;
    }

    public void OnUpdate()
    {
        ChasePlayer();
        CheckDistance();
    }

    void ChasePlayer()
    {
        // �ҵ����
        if (enemy.attackTarget != null)
        {
            // ׷�����
            if (Vector3.Distance(enemy.attackTarget.transform.position, enemyTransform.position) <= enemy.stopDistance)
            {
                // ֹͣ�ƶ�����ʼ����
                enemy.TransitionState(EnemyState.ATTACK);
            }
            else
            {
                // ת�����
                enemyTransform.LookAt(enemy.attackTarget.transform.position);
                enemy.agent.SetDestination(enemy.attackTarget.transform.position);
            }
        }
    }

    // ����
    void CheckDistance()
    {
        if (enemy.attackTarget != null &&
            Vector3.Distance(enemy.attackTarget.transform.position, enemyTransform.position) > enemy.findRange)
        {
            enemy.attackTarget = null;
            enemy.TransitionState(EnemyState.NOTARGET);
        }
    }
}
