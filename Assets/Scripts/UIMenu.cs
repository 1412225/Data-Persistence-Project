using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class UIMenu : MonoBehaviour
{
    public static UIMenu Instance;
    public static string playerName;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public int highScore;
    public string highScoreHolder;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
        if (highScore != 0)
        {
            highScoreText.text = "High Score : " + highScoreHolder + " : " + highScore;
        }

        LoadName();
        nameInput.text = playerName;
    }

    public void StartGame()
    {
        playerName = nameInput.text;
        SaveName();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        SaveName();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScoreHolder;
        public string playerName;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.highScoreHolder = highScoreHolder;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savemenu.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savemenu.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
            highScoreHolder = data.highScoreHolder;
        }
    }

    public void SaveName()
    {
        SaveData data = new SaveData();
        data.playerName = nameInput.text;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedname.json", json);
    }
    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savedname.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            playerName = data.playerName;
        }
    }
}
