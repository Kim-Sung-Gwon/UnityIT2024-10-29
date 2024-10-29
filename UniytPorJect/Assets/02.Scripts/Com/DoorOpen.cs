using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator animator;
    private bool isOpen;
    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            animator.SetBool(OpenClos, value);
        }
    }

    private readonly int OpenClos = Animator.StringToHash("character_nearby");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOpen == false)
        {
            if (other.CompareTag("Player"))
            {
                IsOpen = true;
            }
        }
        StartCoroutine(DoorClose());
    }

    IEnumerator DoorClose()
    {
        yield return new WaitForSeconds(3.0f);
        IsOpen = false;
    }
}
