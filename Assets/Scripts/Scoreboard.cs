using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TMP_Text scoreboardTextObject;
    
    int score = 0;

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreboardTextObject.text = "Score: " + score;
    }
}
