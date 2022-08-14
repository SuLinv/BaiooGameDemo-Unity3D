using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : ActionManager
{
    private InputManager inputManager;
    public float maxCountDown = 0.55f;
    float countDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        inputManager = gameObject.GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0) countDown -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && countDown <= 0 && !isPause)
        {
            inputManager.controller?.StartAttack();
            countDown = maxCountDown;
        }
    }
}
