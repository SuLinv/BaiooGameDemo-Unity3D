using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    // ���ת�����ٶ�
    public float angle = 800;
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame

    void Update()
    {
        if(Input.GetMouseButton(1)) // ��ס�Ҽ���ʱ����������ת��
        {
            Camera.main.transform.LookAt(inputManager.player.transform.position);

            // ��ˮƽ��ת��
            Vector3 axis = inputManager.player.transform.position - Camera.main.transform.position;
            axis = Vector3.Cross(axis, Vector3.down);

            // ��Ҫ������»�����ʱ�����������ת
            Camera.main.transform.RotateAround(inputManager.player.transform.position, axis, -Input.GetAxis("Mouse Y") * angle * Time.deltaTime);
            Camera.main.transform.RotateAround(inputManager.player.transform.position, this.transform.up, Input.GetAxis("Mouse X") * angle * Time.deltaTime);

        }
    }
}
