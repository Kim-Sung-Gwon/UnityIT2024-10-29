using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRot : MonoBehaviour
{
    float rot = 60;

    private void Update()
    {
        transform.Rotate(0, rot * Time.deltaTime, 0);
    }
}
