using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DestroySelf : MonoBehaviour
{
   public float DestroyTimer = 0;

    void Update()
    {
        DestroyTimer += Time.deltaTime;
        if (DestroyTimer > 4)
        {
            if (GameObject.Find("ItemExplanationWindow") != null)
            {
                DestroyImmediate(GameObject.Find("ItemExplanationWindow"));
            }
            Destroy(gameObject);
        }
    }
}
