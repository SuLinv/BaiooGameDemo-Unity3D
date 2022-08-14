using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : ActionManager
{
    InputManager inputManager;

    bool command;

    Vector2 direction;

    void Start()
    {
        base.Start();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if(!command)
        {
            // 查看有没有跳跃指令
            command = Input.GetKeyDown(KeyCode.Space);
            if (command)
                // 有则读取跳跃方向
                direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        command = command && !isPause;    
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (command)
        {
            inputManager.controller?.Jump(direction);
            command = false;
        }
    }
}
