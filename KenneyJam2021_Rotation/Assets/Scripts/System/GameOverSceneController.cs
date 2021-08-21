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
        StartCoroutine(GameOverSceneRoutine());
    }

    private IEnumerator GameOverSceneRoutine()
    {
        if (final_score_readout != null)
        {
            if (GameManager.Instance != null)
            {
                final_score_readout.text = "You survived until the year " + GameManager.Instance.score;
            }
        }

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("TitleScreen");
    }
}
