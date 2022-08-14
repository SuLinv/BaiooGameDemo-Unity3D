using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //首先引入此命名空间

public class GameEvent : MonoBehaviour
{
  
    public AudioSource source1;
    public AudioSource source2;
    public GameObject PauseMusic;
    public GameObject PlayMusic;
    public GameObject IllustratePanel;
    public void startGame()
    {
        //System.LoadSence("Choose");
        Debug.Log("Start");
        source1.Play();
        SceneManager.LoadScene("CharacterChoose");
    }
    public void Illustrate()
    {
        Debug.Log("ILL");

        source1.Play();
        IllustratePanel.SetActive(true);

    }
    public void close()
    {
        source1.Play();
        IllustratePanel.SetActive(false);
    }
    public void Setting()
    {
        Debug.Log("set");
        source1.Play();

    }
    public void Exit()
    {
        source1.Play();
        #if UNITY_EDITOR 
        //如果是在编辑器环境下
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        //在打包出来的环境下
        Application.Quit();
        #endif

        Debug.Log("exit");
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
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
