using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState : IState
{
    EnemyController enemy;

    Transform enemyTransform;

    private float coolDown = 0;

    public AttackState(EnemyController enemyController)
    {
        this.enemy = enemyController;
    }

    public void OnEnter()
    {
        enemy.isWalk = false;
        enemyTransform = enemy.gameObject.transform;
        enemy.Stop();
    }

    public void OnUpdate()
    {
        Attack();
        CheckDistance();
        CheckTarget();

        coolDown -= Time.deltaTime;
    }

    /*�������*/
    void Attack()
    {
        // �����������й���Ŀ�� �� ����С��ֹͣ����
        if (enemy.attackTarget != null &&
            Vector3.Distance(enemy.attackTarget.transform.position, enemyTransform.position) <= enemy.stopDistance &&
            coolDown <= 0)
        {
            // ���Ŷ���
            enemy.ani.SetTrigger("Attack");
            // ����
            enemy.attackTarget.GetComponent<PlayerController>().Hit(enemy.damage);
            coolDown = enemy.maxCoolDown;
        }
    }

    void CheckDistance()
    {
        if (enemy.attackTarget != null &&
            Vector3.Distance(enemy.attackTarget.transform.position, enemyTransform.position) > enemy.stopDistance)
        {
            enemy.TransitionState(EnemyState.CHASE);
        }
    }

    void CheckTarget()
    {
        if(enemy.attackTarget == null)
        {
            enemy.TransitionState(EnemyState.NOTARGET);
        }
    }
}
