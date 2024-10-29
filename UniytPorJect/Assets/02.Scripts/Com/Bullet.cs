using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private TrailRenderer Trand;
    public float moveSpeed = 2500f;
    public float Damage = 25f;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        Trand = GetComponent<TrailRenderer>();
        Invoke("BulletDisable", 3.0f);
    }

    public void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * moveSpeed);
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
