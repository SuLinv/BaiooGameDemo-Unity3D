using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


//��ʼui->main�������Ĳ�������
//����
//���뵽main����ݲ����ж��Ƿ����
public class StartToMain : MonoBehaviour
{
    public static StartToMain Instance { get; private set; }
    public int param { get; set; } = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadGame()
    {
        Instance.param = 1;
        SceneManager.LoadScene("main");
    }
    public static void StartGame()
    {

        SceneManager.LoadScene("main");
    }


}
