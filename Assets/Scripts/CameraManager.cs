using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    // 相机转动角速度
    public float angle = 800;
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame

    void Update()
    {
        if(Input.GetMouseButton(1)) // 按住右键的时候控制摄像机转动
        {
            Camera.main.transform.LookAt(inputManager.player.transform.position);

            // 求水平的转轴
            Vector3 axis = inputManager.player.transform.position - Camera.main.transform.position;
            axis = Vector3.Cross(axis, Vector3.down);

            // 需要鼠标向下滑动的时候摄像机向上转
            Camera.main.transform.RotateAround(inputManager.player.transform.position, axis, -Input.GetAxis("Mouse Y") * angle * Time.deltaTime);
            Camera.main.transform.RotateAround(inputManager.player.transform.position, this.transform.up, Input.GetAxis("Mouse X") * angle * Time.deltaTime);

        }
    }
}
