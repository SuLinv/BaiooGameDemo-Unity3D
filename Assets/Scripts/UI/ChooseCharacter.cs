using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacter : MonoBehaviour
{
    private int Charindex ;
    public GameObject male;
    public GameObject female;

    void Start()
    {
        Debug.Log(111);
        Charindex = PlayerPrefs.GetInt("SelectCharacterIndex");
        Debug.Log(Charindex);
        if(Charindex == 0)
        {
           male.SetActive(true);
        }
        else if(Charindex == 1)
        {
            female.SetActive(true);
        }
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
