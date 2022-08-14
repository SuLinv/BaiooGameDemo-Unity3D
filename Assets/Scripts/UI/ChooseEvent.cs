using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //首先引入此命名空间


public class ChooseEvent : MonoBehaviour
{ 
    
    //public GameObject[] characterGameObjects;//实例化之后存储进去；
    public GameObject male;
    public GameObject female;
    public GameObject rigth;
    public GameObject left;
    public AudioSource source1;
    public AudioSource source2;
    public GameObject PauseMusic;
    public GameObject PlayMusic;

 
    private int selectIndex = 0;//哪个角色被选中；
    private int length;//suo
    
    //3d人物旋转
    // private Vector2 currentPos;
    // private Vector2 lastPos;
    // [Range(5, 50)] public float rotateSpeed = 15;
    // void Update()
    // {
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         lastPos = Input.mousePosition;
    //     }
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         currentPos = Input.mousePosition;
    //         if(selectIndex == 0)
    //         {
    //             male.transform.Rotate(Vector3.up, (lastPos.x - currentPos.x) * Time.deltaTime);
    //         }else
    //         {
    //             female.transform.Rotate(Vector3.up, (lastPos.x - currentPos.x) * Time.deltaTime);                
    //         }
            
    //     }
    // }
    public void OnPrevButton()
    {
        selectIndex = 0;
        male.SetActive(true);
        rigth.SetActive(true);
        female.SetActive(false);
        left.SetActive(false);
        Debug.Log("Left");
        source1.Play();
    }
    public void OnNextButton()
    {   selectIndex = 1;
        male.SetActive(false);
        rigth.SetActive(false);
        female.SetActive(true);
        left.SetActive(true);
        Debug.Log("Right");
        source1.Play();

    }


    public void OnChooseButton()
    {
        PlayerPrefs.SetInt("SelectCharacterIndex" , selectIndex);//存储选择的角色
        SceneManager.LoadScene("main");
        //SceneManager.LoadScene("New Scene");

        source1.Play();

    }
      public void playBGM()
    {
      source2.Play();
      PauseMusic.SetActive(true);
      PlayMusic.SetActive(false);

    }
    public void pauseBGM()
    {
        source2.Pause();
        PauseMusic.SetActive(false);
        PlayMusic.SetActive(true);
    }
}
