using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet P_bullet;

    [SerializeField] private Transform tr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] public TrailRenderer Trand;
    public float moveSpeed = 2500f;
    public float Damage;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        Trand = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * moveSpeed);
        StartCoroutine(BulletDisable());
    }

    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
        Trand.Clear();
    }

    private void OnCollisionEnter(Collision col)
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Trand.Clear();
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
