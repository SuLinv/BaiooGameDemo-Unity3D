using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
//ֻ������player������
namespace SaveSystemTutorial
{

    public class SavePoint : MonoBehaviour
    {

        GameObject player;
        PlayerController playerContro;

        public AudioSource music;

        //Ҫ��������ݣ���������Ҫ���ǿ������л��洢
        [System.Serializable]
        class SaveData
        {
            //�����Ϣ
            //public GameObject player;
           // public PlayerController playerContro;
            public List<float> maxAttribute; //�洢ÿ���������ֵ��0-health
               // float health;
            public List<float> attribute; //�洢ÿ�����ԣ�0-health   
            public Vector3 playerPosition;
            public int coin = 0;
          
        }
        //������ļ���
        const string PLAYER_DATA_KEY = "PlayerData";
        const string PLAYER_DATA_FILE_NAME = "PlayerData.sav";

        // Start is called before the first frame update
        void Start()
        {
            player= GameObject.Find("MaleCharacter");
            playerContro = player.GetComponent<PlayerController>();
            music = GetComponent<AudioSource>();

            //�����Ļ������￪ʼ��������
             //if (StartToMain.Instance.param == 1)
            // {
             //   var path = Path.Combine(Application.persistentDataPath, PLAYER_DATA_FILE_NAME);
             //   if (File.Exists(path))
             //   {
             //       Load();
             //   }
             
             //}
        }

        // Update is called once per frame
        void Update()
        {

        }
        //���ﱣ����Զ��浵��������Ч
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                playerContro=other.GetComponent<PlayerController>();
                Save();
                
                music.Play();
            }
        }


        #region Save and Load

        public void Save()
        {
            // SaveByPlayerPrefs();
            SaveByJson();
        }

        public void Load()
        {
            // LoadFromPlayerPrefs();
            LoadFromJson();
        }

        #endregion

        #region PlayerPrefs

        void SaveByPlayerPrefs()
        {
            SaveSystem.SaveByPlayerPrefs(PLAYER_DATA_KEY, SavingData());
        }

        void LoadFromPlayerPrefs()
        {
            var json = SaveSystem.LoadFromPlayerPrefs(PLAYER_DATA_KEY);
            var saveData = JsonUtility.FromJson<SaveData>(json);
            LoadData(saveData);
        }

        #endregion

        #region JSON

        void SaveByJson()
        {
            SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, SavingData());
            // SaveSystem.SaveByJson($"{System.DateTime.Now:yyyy.dd.M HH-mm-ss}.sav", SavingData());
        }

        void LoadFromJson()
        {
            var saveData = SaveSystem.LoadFromJson<SaveData>(PLAYER_DATA_FILE_NAME);

            LoadData(saveData);
        }

        #endregion

        #region Help Functions

        SaveData SavingData()
        {
            var saveData = new SaveData();

         
            saveData.playerPosition = transform.position;
            saveData.coin = playerContro.coin;
            saveData.maxAttribute = playerContro.maxAttribute;
            saveData.attribute= playerContro.attribute;
            return saveData;
        }

        void LoadData(SaveData saveData)
        {
            player.transform.position = saveData.playerPosition;
            playerContro.coin=saveData.coin ;
            playerContro.maxAttribute=saveData.maxAttribute;
            playerContro.attribute=saveData.attribute ;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Developer/Delete Player Data Prefs")]
        public static void DeletePlayerDataPrefs()
        {
            PlayerPrefs.DeleteKey(PLAYER_DATA_KEY);
        }

        [UnityEditor.MenuItem("Developer/Delete Player Data Save File")]
        public static void DeletePlayerDataSaveFile()
        {
            SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME);
        }
#endif

        #endregion
    }
}

