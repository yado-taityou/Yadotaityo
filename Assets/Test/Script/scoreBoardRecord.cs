using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class scoreBoardRecord : MonoBehaviour
{
    private const string SaveKeyPrefix = "HighScores_"; // 難易度ごとのキーのプレフィックス
    private Dictionary<int, List<int>> highScores = new Dictionary<int, List<int>>()
    {
        { 0, new List<int>() }, // Easy
        { 1, new List<int>() }, // Normal
        { 2, new List<int>() }  // Hard
    };

    public TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[3];

    void Start()
    {
        LoadHighScores(); // 起動時に全難易度のデータをロード
        for (int i = 0;i < scoreTexts.Length;i++)
        {
            UpdateScoreDisplay(i);
        }
    }

    private void LoadHighScores()
    {
        List<int> difficultyLevels = new List<int>(highScores.Keys); // キーのコピーを作成

        foreach (int difficulty in difficultyLevels)
        {
            string key = SaveKeyPrefix + difficulty;
            if (ES3.KeyExists(key))
            {
                highScores[difficulty] = ES3.Load<List<int>>(key);
            }
        }
    }

    // 指定した難易度のランキングを文字列リストとして取得
    public List<string> GetHighScoresAsString(int difficulty = 0)
    {
        List<string> scoreStrings = new List<string>();

        if (!highScores.ContainsKey(difficulty)) return scoreStrings;

        foreach (int time in highScores[difficulty])
        {
            int minutes = (time / 60000);
            int seconds = (time / 1000) % 60;
            int milliseconds = (time % 1000) / 10;
            scoreStrings.Add($"{minutes:00}:{seconds:00}:{milliseconds:00}");
        }

        return scoreStrings;
    }

    // UIにスコアを表示（難易度ごと、外部から呼び出し可能）
    public void UpdateScoreDisplay(int difficulty = 0)
    {
        List<string> scores = GetHighScoresAsString(difficulty);
        scoreTexts[difficulty].text = "";
        for (int i = 0; i < scores.Count; i++)
        {
                if (scoreTexts[i] != null)
                {
                    scoreTexts[difficulty].text += scores[i]+"\n"; // スコアを表示
                }
                else
                {
                    scoreTexts[difficulty].text += "--:--:--\n"; // データなしのとき
                }
        }
    }
}
