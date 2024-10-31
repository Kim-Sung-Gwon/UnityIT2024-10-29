using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private Transform tr;
    private CharacterController cc;
    private PlayerAnimator playerAnimator;
    private Transform cameraTr;
    private FireCtrl fireCtrl;
    private GameManager gameManager;
    private FollowCam F_cam;
    private UIManager uimanager;
    private Transform CamRot;

    Vector3 moveDir = Vector3.zero;
    Vector2 mouseDelta;

    float gravity = -10f;
    float rotSpeed = 7f;

    float currentSpeed;
    float moveSpeed = 7f;
    float runSpeed = 14f;

    bool running;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        tr = transform;
        cc = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        fireCtrl = GetComponent<FireCtrl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraTr = Camera.main.transform;
        CamRot = GameObject.FindWithTag("Player").transform.GetChild(3).GetChild(1).GetComponent<Transform>();
        F_cam = GameObject.Find("Main Camera").GetComponent<FollowCam>();
        currentSpeed = moveSpeed;
        uimanager = GameObject.Find("UIManager").GetComponent <UIManager>();
    }

    void FixedUpdate()
    {
        if (moveDir != Vector3.zero)
        {
            Vector3 forward = Vector3.ProjectOnPlane(cameraTr.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(cameraTr.right, Vector3.up).normalized;

            Vector3 moveDirection = (forward * moveDir.z + right * moveDir.x).normalized;
            cc.Move(moveDirection * currentSpeed * Time.deltaTime);
            
            // 달리기시 중력 표현
            cc.Move(new Vector3(0, gravity, 0) * Time.deltaTime);
        }
    }

    void OnRotation(InputValue value)
    {
        mouseDelta = value.Get<Vector2>();
        tr.Rotate(Vector3.up * rotSpeed * mouseDelta.x * Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        playerAnimator.MoveAnimation(dir);
    }

    void OnRun()
    {
        ZoomPos();
        playerAnimator.RunAnimation(!playerAnimator.IsRun);
        running = !running;

        // 달리기 중인지 판단하고 그에 따른 속도 변환
        currentSpeed = running ? runSpeed : moveSpeed;
    }

    void OnJump()
    {
        playerAnimator.JumpAnimation();
    }

    void OnReload()
    {
        fireCtrl.ReloadPlay();
        playerAnimator.ReloadAnimation();
    }

    public void OnFire()
    {
        StartCoroutine(FireClick());
    }

    IEnumerator FireClick()
    {
        yield return new WaitForSeconds(0.15f);
        fireCtrl.Fire();
        playerAnimator.FireAnimation();
    }

    void OnZoom()
    {
        F_cam.zoomCam = !F_cam.zoomCam;

        if (F_cam.zoomCam)
        {
            F_cam.CamTwoOn();
            uimanager.Zoom_Image.gameObject.SetActive(true);
            currentSpeed = 1;
        }
        else
        {
            ZoomPos();
        }
    }

    private void ZoomPos()
    {
        F_cam.zoomCam = true;
        F_cam.CamOneOn();
        uimanager.Zoom_Image.gameObject.SetActive(false);
        currentSpeed = moveSpeed;
    }

    void OnCursorOn()
    {
        Cursor.visible = true;
    }
}
