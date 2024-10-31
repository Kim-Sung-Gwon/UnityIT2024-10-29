using UnityEngine;

public class HpBarLook : MonoBehaviour
{
    private Canvas HPbar;
    private Transform Player;

    void Start()
    {
        HPbar = GetComponent<Canvas>();
        Player = GameObject.FindWithTag("Player").transform.GetComponent<Transform>();
    }

    void Update()
    {
        HPbar.transform.LookAt(Player.position);
    }
}
