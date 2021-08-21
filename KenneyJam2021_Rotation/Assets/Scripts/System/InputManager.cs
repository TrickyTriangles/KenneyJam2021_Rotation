using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameManager game_manager;
    public bool can_select = true;

    private void Start()
    {
        if (game_manager != null)
        {

        }
    }

    private void GameManager_GameEnd()
    {
        can_select = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && can_select)
        {
            CheckForSelectableObject();
        }
    }

    private bool CheckForSelectableObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null)
        {
            if (hit.collider.CompareTag("Unit"))
            {
                //PlayerUnit selection = hit.collider.GetComponent<PlayerUnit>();
                return true;
            }
        }

        return false;
    }
}
