using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public static float currentScore;

    [SerializeField] TextMeshProUGUI scoreText;

    #region On Enable/Disable
    private void OnEnable()
    {
        EnemyHealth.OnEnemyDied += EnemyDied;
    }
    private void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= EnemyDied;
    }
    #endregion


    void ScoreChanged()
    {
        //Update UI
        scoreText.text = "" + currentScore.ToString("00000");
    }


    public void EnemyDied(GameObject enemy, float scoreChange)
    {
        Debug.Log("ping");

        
        currentScore += scoreChange;

        ScoreChanged();
    }


    public void SetScore(float newScore)
    {
        currentScore = newScore;
        
        ScoreChanged();
    }
}
