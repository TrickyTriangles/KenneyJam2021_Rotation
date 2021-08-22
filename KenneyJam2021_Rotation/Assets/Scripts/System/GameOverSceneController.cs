using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI final_score_readout;

    private void Start()
    {
        if (final_score_readout != null)
        {
            if (GameManager.Instance != null)
            {
                final_score_readout.text = GameManager.Instance.Score.ToString();
            }
        }
    }

    public void ContinueButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
