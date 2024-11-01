using UnityEngine;

public class ItemRot : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);
    }
}
