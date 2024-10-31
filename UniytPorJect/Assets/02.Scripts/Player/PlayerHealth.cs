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
        IIitem item = other.GetComponent<IIitem>();
        if (item != null)
        {
            item.Use(gameObject);
            fireCtrl.BulletUpdata();
        }
    }
}
