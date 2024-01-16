using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text highScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    public int highestScore = 0;
    private string highScoreHolder;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = "Score : " + UIMenu.playerName + " : " + m_Points;
        LoadHighScore();
        UpdateHighScore();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = "Score : " + UIMenu.playerName + " : " + m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > highestScore)
        {
            highestScore = m_Points;
            UIMenu.Instance.highScore = m_Points;
            UIMenu.Instance.highScoreHolder = UIMenu.playerName;
            UIMenu.Instance.SaveHighScore();
            highScoreHolder = UIMenu.playerName;
            highScoreText.text = "Best Score : " + highScoreHolder + " : " + highestScore;
            SaveHighScore();
        }
    }

    //writes down highscore
    void UpdateHighScore()
    {
        if (highestScore != 0)
        {
            highScoreText.text = "Best Score : " + highScoreHolder + " : " + highestScore;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highestScore;
        public string highScoreHolder;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highestScore = highestScore;
        data.highScoreHolder = highScoreHolder;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highestScore = data.highestScore;
            highScoreHolder = data.highScoreHolder;
        }
    }
}
