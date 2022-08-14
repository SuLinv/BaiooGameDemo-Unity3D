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
        // 找到玩家
        if (enemy.attackTarget != null)
        {
            // 追到玩家
            if (Vector3.Distance(enemy.attackTarget.transform.position, enemyTransform.position) <= enemy.stopDistance)
            {
                // 停止移动并开始攻击
                enemy.TransitionState(EnemyState.ATTACK);
            }
            else
            {
                // 转向玩家
                enemyTransform.LookAt(enemy.attackTarget.transform.position);
                enemy.agent.SetDestination(enemy.attackTarget.transform.position);
            }
        }
    }

    // 拉脱
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
