using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoTargetState : IState
{

    EnemyController enemy;

    Transform enemyTransform;

    public NoTargetState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemyTransform = enemy.gameObject.transform;
    }

    public void OnUpdate()
    {
        FindPlayer();
        if(enemy.attackTarget == null)
        BackHome();
    }

    /*��Χ�������*/
    void FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, enemy.findRange, enemy.searchLayer, QueryTriggerInteraction.UseGlobal);
        foreach (Collider collider in colliders)
        {
            // ��Χ�ڷ��������
            if (collider.GetComponent<PlayerController>() != null)
            {
                // ���Ϊ��������
                enemy.attackTarget = collider.gameObject;
                // ת��״̬
                enemy.TransitionState(EnemyState.CHASE);
                return;
            }
        }
    }

    private void BackHome()
    {
        if (Vector3.Distance(enemyTransform.position, enemy.oriPosition) > enemy.stopDistance)
        {
            enemyTransform.LookAt(enemy.oriPosition);
            enemy.agent.SetDestination(enemy.oriPosition);
        }
        else
        {
            enemyTransform.rotation = enemy.oriRotation;
            enemy.Stop();
        }
    }
}
