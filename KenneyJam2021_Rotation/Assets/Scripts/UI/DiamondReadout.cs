using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiamondReadout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Subscribe_MoneyUpdated(GameManager_MoneyUpdated);
        }
    }

    private void GameManager_MoneyUpdated(int new_value)
    {
        if (text != null)
        {
            text.text = new_value.ToString();
        }
    }
}
