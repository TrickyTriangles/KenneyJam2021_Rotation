using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YearReadout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Subscribe_ScoreUpdated(GameManager_ScoreUpdated);
        }
    }

    private void GameManager_ScoreUpdated(int new_score)
    {
        if (text != null)
        {
            text.text = "Year " + new_score.ToString();
        }
    }
}
