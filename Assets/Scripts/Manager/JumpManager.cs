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
            // �鿴��û����Ծָ��
            command = Input.GetKeyDown(KeyCode.Space);
            if (command)
                // �����ȡ��Ծ����
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
