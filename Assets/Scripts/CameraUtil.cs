using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������ز���������
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

    // ��ȡ��ǰ���������
    public Vector2 getForward()
    {
        // �����ָ����Ϸ��ɫ������
        Vector3 forward3D = gameObject.transform.position - Camera.main.transform.position;
        // 3ά������ xz ƽ���ͶӰ
        Vector2 forward = new Vector2(forward3D.x, forward3D.z);
        forward.Normalize();
        return forward;
    }


}
