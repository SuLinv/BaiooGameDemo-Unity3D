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

    /*¹¥»÷Íæ¼Ò*/
    void Attack()
    {
        // ¹¥»÷Ìõ¼þ£ºÓÐ¹¥»÷Ä¿±ê ÇÒ ¾àÀëÐ¡ÓÚÍ£Ö¹¾àÀë
        if (enemy.attackTarget != null &&
            Vector3.Distance(enemy.attackTarget.transform.position, enemyTransform.position) <= enemy.stopDistance &&
            coolDown <= 0)
        {
            // ²¥·Å¶¯»­
            enemy.ani.SetTrigger("Attack");
            // ¹¥»÷
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
