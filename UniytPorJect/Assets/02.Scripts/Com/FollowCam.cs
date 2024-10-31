using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCam : MonoBehaviour
{
    private Transform target;
    private Transform Cam;

    private CinemachineVirtualCamera virtualCam;
    private CinemachineTransposer transposer;

    float Height;
    float Distance = 7f;
    float movedamping = 15f;
    float rotdamping = 15f;
    float targetOffset = 2.0f;

    float maxHeight = 12f;
    float castOffset = 1.0f;
    float originHeight;
    float heightChangeThreshold = 0.1f;

    public Camera MainCam;
    public Camera ZoomCam;
    public bool zoomCam = true;

    IEnumerator Start()
    {
        Cam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        MainCam = Camera.main;
        ZoomCam = GameObject.FindWithTag("Player").transform.GetChild(3).GetChild(1).GetComponent<Camera>();
        CamOneOn();

        virtualCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        if (virtualCam != null)
        {
            transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                originHeight = transposer.m_FollowOffset.y;
                Height = originHeight;
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    private void Update()
    {
        if (target == null) return;
        Vector3 castTarget = target.position + (target.up * castOffset);
        Vector3 castDir = (castTarget - virtualCam.transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(virtualCam.transform.position, castDir, out hit, Mathf.Infinity))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                Height = Mathf.Lerp(Height, maxHeight, Time.deltaTime);
            }
            else
                Height = Mathf.Lerp(Height, originHeight, Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        if (transposer == null) return;

        // transposer를 통해 버추얼 카메라의 높이를 변경
        Vector3 followOffset = transposer.m_FollowOffset;
        followOffset.y = Height;
        transposer.m_FollowOffset = followOffset;

        // 버추얼 카메라가 회전을 하는데 기준이될 대상
        virtualCam.transform.rotation = Quaternion.Slerp(virtualCam.transform.rotation, target.rotation, Time.deltaTime * rotdamping);

        // 버추얼 카메라가 따라갈 대상
        virtualCam.LookAt = target;
    }

    public void CamOneOn()
    {
        MainCam.enabled = true;
        ZoomCam.enabled = false;
    }

    public void CamTwoOn()
    {
        MainCam.enabled = false;
        ZoomCam.enabled = true;
    }
}
