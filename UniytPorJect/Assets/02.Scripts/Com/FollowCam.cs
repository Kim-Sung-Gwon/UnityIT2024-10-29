using System.Collections;
using UnityEngine;
using Cinemachine;

public class FollowCam : MonoBehaviour
{
    private Transform target;
    private Transform Cam;

    private CinemachineVirtualCamera virtualCam;
    private CinemachineTransposer transposer;

    float Height;
    const float Distance = 7f;
    const float movedamping = 15f;
    const float rotdamping = 15f;
    const float targetOffset = 2.0f;
    const float maxHeight = 12f;
    const float castOffset = 1.0f;
    const float heightChangeThreshold = 0.1f;

    float originHeight;

    public Camera MainCam;
    public Camera ZoomCam;
    public bool zoomCam = true;

    IEnumerator Start()
    {
        Cam = Camera.main.transform;
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        MainCam = Camera.main;
        ZoomCam = target.GetChild(3).GetChild(1).GetComponent<Camera>();
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

        if (Physics.Raycast(virtualCam.transform.position, castDir, out RaycastHit hit, Mathf.Infinity))
        {
            Height = !hit.collider.CompareTag("Player") ?
                Mathf.Lerp(Height, maxHeight, Time.deltaTime) :
                Mathf.Lerp(Height, originHeight, Time.deltaTime);
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
