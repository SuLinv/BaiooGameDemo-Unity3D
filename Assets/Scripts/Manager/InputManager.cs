using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [HideInInspector]
    public GameObject player;
    private PlayerController Controller;
    public PlayerController controller
    {
        get { return Controller; }
    }

    public List<ActionManager> actonList;


    private void Update()
    {
        if(player != null)
        {
            // ���������ж�
            if(player.transform.position.y <= transform.position.y)
            {
                controller?.Die();
            }
        }
    }

    public List<EnemyController> enemyList;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }

        actonList = new List<ActionManager>();

    }

    public void RegisteTo(GameObject player)
    {
        this.player = player;
        Controller = player.GetComponent<PlayerController>();
        controller.AfterDie += OnPlayerDie;
    }

    public void RegisteEnemy(EnemyController controller)
    {
        if(enemyList == null)
        {
            enemyList = new List<EnemyController>();
        }

        enemyList.Add(controller);
    }

    void OnPlayerDie()
    {
        this.Controller = null;
        foreach(EnemyController enemy in enemyList)
        {
            enemy.OnPlayerDie();
        }
    }

    public void PauseTheGame()
    {
        Time.timeScale = 0;
        foreach( ActionManager action in actonList)
        {
            action.isPause = true;
        }
    }

    public void UnPauseTheGame()
    {
        Time.timeScale = 1;
        foreach (ActionManager action in actonList)
        {
            action.isPause = false;
        }
    }

}
