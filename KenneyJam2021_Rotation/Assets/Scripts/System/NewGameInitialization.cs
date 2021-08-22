using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameInitialization : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.NewGameInitialization();
    }
}
