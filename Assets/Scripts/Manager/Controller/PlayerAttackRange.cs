using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    public static PlayerAttackRange instance;
    public HashSet<Transform> transInAttackRange; //存放进入攻击范围的transform

    private void Awake() {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        transInAttackRange = new HashSet<Transform>();
    }

    private void OnTriggerEnter(Collider enemy) {
        if(!transInAttackRange.Contains(enemy.transform)){
            transInAttackRange.Add(enemy.transform);
        }
        Debug.Log(enemy.name);
    }

    private void OnTriggerExit(Collider enemy) {
        if(transInAttackRange.Contains(enemy.transform))
            transInAttackRange.Remove(enemy.transform);
    }
}
