using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet P_bullet;

    private Rigidbody rb;
    private TrailRenderer Trand;
    public float moveSpeed = 2500f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Trand = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * moveSpeed);
        StartCoroutine(BulletDisable());
    }

    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(3);
        DisableBullet();
    }

    private void OnCollisionEnter(Collision col)
    {
        DisableBullet();
    }

    void DisableBullet()
    {
        Trand.Clear();
        rb.Sleep();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }
}
