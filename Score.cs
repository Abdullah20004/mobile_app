using UnityEngine;
using TMPro; 

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 

    void Update()
    {
        if (scoreText != null)
        {
            float currentScore = Time.timeSinceLevelLoad;
            scoreText.text = currentScore.ToString("0");
        }
    }
}