using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private Camera _camera;
    public bool can_select = true;
    public PlayerUnit selected_unit;
    public Task selected_task;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && can_select)
        {
            if (CheckForSelectableObject())
            {
                if (selected_unit != null & selected_task != null)
                {
                    BeginUnitMovement();
                    DeselectAllObjects();
                }
            }
            else
            {
                DeselectAllObjects();
            }
        }
    }

    private bool CheckForSelectableObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null)
        {
            if (hit.collider.CompareTag("Unit"))
            {
                PlayerUnit selection = hit.collider.GetComponent<PlayerUnit>();
                selected_unit = selection != null ? selection : selected_unit;
                return true;
            }
            else if (hit.collider.CompareTag("Task"))
            {
                Task selection = hit.collider.GetComponent<Task>();
                selected_task = selection != null ? selection : selected_task;
                return true;
            }
        }

        return false;
    }

    private void BeginUnitMovement()
    {
        selected_unit.MoveUnit(selected_task);
    }

    private void DeselectAllObjects()
    {
        selected_unit = null;
        selected_task = null;
    }
}
