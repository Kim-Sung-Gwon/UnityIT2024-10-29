using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private FireCtrl fireCtrl;

    private void Start()
    {
        fireCtrl = GetComponent<FireCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Iitem item = other.GetComponent<Iitem>();
        if (item != null)
        {
            item.Use(gameObject);
            fireCtrl.BulletUpdata();
        }
    }
}
