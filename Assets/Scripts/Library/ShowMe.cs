using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShowMe : MonoBehaviour
{
    public float ShowTimer = 0;
    bool IsShow = true;

    public void SetShowTimer(float timer)
    {
        ShowTimer = timer;
        IsShow = true;
    }

    void Update()
    {
        ShowTimer += Time.deltaTime;
        if (ShowTimer > 1)
        {
            ShowTimer = 0;
            IsShow = !IsShow;

            foreach (Component g in gameObject.GetComponentsInChildren(typeof(Component), true))
            {
                if (g.gameObject.name != gameObject.name)
                {
                    //Debug.LogError(g.gameObject.name + " " + gameObject.name);
                    g.gameObject.SetActive(IsShow);
                }
            }
        }
    }
}
