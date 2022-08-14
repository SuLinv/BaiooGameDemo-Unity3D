using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 摄像机相关参数单例类
public class CameraUtil : MonoBehaviour 
{
    public static CameraUtil instance;

    public static CameraUtil Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // 获取当前摄像机朝向
    public Vector2 getForward()
    {
        // 摄像机指向游戏角色的向量
        Vector3 forward3D = gameObject.transform.position - Camera.main.transform.position;
        // 3维向量在 xz 平面的投影
        Vector2 forward = new Vector2(forward3D.x, forward3D.z);
        forward.Normalize();
        return forward;
    }


}
