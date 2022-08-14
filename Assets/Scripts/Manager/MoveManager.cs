using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : ActionManager
{
    InputManager inputManager;
    Vector2 moveDirection = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        inputManager = gameObject.GetComponent<InputManager>();
    }

    private void Update()
    {
        if (Mathf.Approximately(moveDirection.magnitude, 0) && !isPause)
        {
            moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            // �õ��ƶ�����
            moveDirection.Normalize();
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // ���ƶ�ָ��
        if (!(Mathf.Approximately(moveDirection.magnitude, 0.0f)))
        {
            inputManager.controller?.Move(moveDirection);
            moveDirection = Vector2.zero;
        }
        else
            // û���ƶ�ָ�ֹͣ�˶�
           inputManager.controller?.StopMoving();
    }
}
