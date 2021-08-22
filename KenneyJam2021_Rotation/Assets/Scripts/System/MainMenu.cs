using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool can_select = true;
    [SerializeField] private Canvas main_canvas;
    [SerializeField] private Canvas instructions_canvas;

    private void Start()
    {
        if (main_canvas != null)
        {
            main_canvas.gameObject.SetActive(true);
        }

        if (instructions_canvas != null)
        {
            instructions_canvas.gameObject.SetActive(false);
        }
    }

    public void StartButtonClicked()
    {
        if (can_select)
        {
            can_select = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.BeginNewGame();
                SoundManagerScript.PlaySound("StartGameButton");
            }

            SceneManager.LoadScene("SampleScene");
        }
    }

    public void InstructionsButtonClicked()
    {
        if (can_select)
        {
            SoundManagerScript.PlaySound("StartGameButton");
            main_canvas.gameObject.SetActive(false);
            instructions_canvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Unused
    /// </summary>
    public void ReturnButtonClicked()
    {
        main_canvas.gameObject.SetActive(true);
        instructions_canvas.gameObject.SetActive(false);
    }
}
