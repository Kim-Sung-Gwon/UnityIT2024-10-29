using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarLook : MonoBehaviour
{
    private Canvas HPbar;
    private Transform CamTr;

    void Start()
    {
        HPbar = GetComponent<Canvas>();
        CamTr = GameObject.Find("Main Camera").transform.GetComponent<Transform>();
    }

    void Update()
    {
        HPbar.transform.LookAt(CamTr);
    }
}
