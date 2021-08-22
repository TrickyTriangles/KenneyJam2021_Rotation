using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitSpawnerButton : MonoBehaviour
{
    [SerializeField] private GameObject unit_to_spawn;
    [SerializeField] private Transform spawn_transform;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI cost_readout;
    [SerializeField] private int unit_cost = 1;
    private bool can_press = true;

    private void Start()
    {
        if (cost_readout != null)
        {
            cost_readout.text = "- " + unit_cost.ToString();
        }
    }

    public void ClickButton()
    {
        if (can_press && GameManager.Instance.Money >= unit_cost)
        {
            StartCoroutine(ButtonPressedRoutine());
        }
    }

    private IEnumerator ButtonPressedRoutine()
    {
        can_press = false;
        GameManager.Instance.Money -= unit_cost;
        SoundManagerScript.PlaySound("BuyUnit");

        if (anim != null) { anim.Play("Pressed", 0); }
        yield return null;

        if (unit_to_spawn != null)
        {
            Instantiate(unit_to_spawn, Vector3.zero, spawn_transform.rotation);
        }

        float timer = 0f;
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        while (timer < info.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        anim.Play("Idle", 0);
        can_press = true;
    }
}
