using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCam : MonoBehaviour
{
    private Transform target;
    [SerializeField] private CinemachineVirtualCamera VirtualCam;
    [SerializeField] private CinemachineTransposer transposer;
    [SerializeField] private Transform Cam;

    float distance = 7.0f;
    float movedamping = 10f;
    float rotdamping = 15f;

    float MaxHeight = 12f;
    float castOffset = 1.7f;
    float originHeight;
    float heightChangeThreshold = 0.1f;

    public Camera MainCam;
    public Camera ZoomCam;
    public bool zoomCam = true;

    IEnumerator Start()
    {
        VirtualCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        Cam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();

        MainCam = Camera.main;
        ZoomCam = GameObject.FindWithTag("Player").transform.GetChild(3).GetChild(1).GetComponent<Camera>();
        CamOneOn();

        if (VirtualCam != null)
        {
            transposer = VirtualCam.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                originHeight = transposer.m_FollowOffset.y;
            }
        }
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        yield return new WaitForSeconds(0.5f);
    }

    void LateUpdate()
    {
        #region 시네머신을 활용하지 않고 카메라 시야에 장해물 감지시 위로 올리는 로직
        //if (target == null) return;
        //Vector3 castTarget = target.position + (target.up * castOffset);
        //Vector3 castDir = (castTarget - Cam.position).normalized;

        //RaycastHit hit;
        //if (Physics.Raycast(Cam.position, castDir, out hit))
        //{
        //    if (!hit.collider.CompareTag("Player"))
        //    {
        //        Height = Mathf.Lerp(Height, MaxHeight, Time.deltaTime);
        //    }
        //    else
        //        Height = Mathf.Lerp(Height, originHeight, Time.deltaTime);
        //}

        //var camPos = target.position - (target.forward * distance) + (target.up * Height);
        //Cam.position = Vector3.Slerp(Cam.position, camPos, movedamping);
        //Cam.rotation = Quaternion.Slerp(Cam.rotation, target.rotation, rotdamping);
        //Cam.LookAt(target.position + (target.up * targetOffset));
        #endregion

        if (VirtualCam == null || target == null || transposer == null) return;
        Vector3 castTarget = target.position + (target.up * castOffset);
        Vector3 castDir = (castTarget - VirtualCam.transform.position).normalized;
        RaycastHit hit;
        float targetHeight = originHeight;
        if (Physics.Raycast(VirtualCam.transform.position, castDir, out hit, distance))
        {
            Debug.DrawLine(VirtualCam.transform.position, hit.point, Color.green);
            if (!hit.collider.CompareTag("Player"))
            {
                targetHeight = MaxHeight;
            }
            if (hit.collider.CompareTag("DamageZone") || hit.collider.CompareTag("NecromDamage"))
            {
                targetHeight = transposer.m_FollowOffset.y;
            }
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Necroman"))
            {
                targetHeight = transposer.m_FollowOffset.y;
            }
        }
        if (Mathf.Abs(transposer.m_FollowOffset.y - targetHeight) >= heightChangeThreshold)
        {
            transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y,
                targetHeight, Time.deltaTime * movedamping);
        }

        Vector3 desirdePosition = target.position - (target.forward * distance)
            + (target.up * transposer.m_FollowOffset.y);
        VirtualCam.transform.position = Vector3.Slerp(VirtualCam.transform.position,
            desirdePosition, Time.deltaTime * movedamping);
        VirtualCam.transform.rotation = Quaternion.Slerp(VirtualCam.transform.rotation,
            target.rotation, Time.deltaTime * rotdamping);
        VirtualCam.LookAt = target;
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
