using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    [Header("Smooth Move")]
    public float moveDuration = 0.6f;
    public AnimationCurve moveCurve;

    [Header("Camera Limit")]
    public float minY = 0f;   // 카메라가 내려갈 수 있는 최소 Y

    private Camera cam;
    private float camHeight;
    private float camWidth;

    private Vector3 currentCamPos;
    private bool isMoving = false;

    void Start()
    {
        cam = Camera.main;

        camHeight = cam.orthographicSize * 2f;
        camWidth = camHeight * cam.aspect;

        currentCamPos = transform.position;

        if (moveCurve == null || moveCurve.length == 0)
        {
            moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
    }

    void LateUpdate()
    {
        if (!isMoving)
            CheckCameraMove();
    }

    void CheckCameraMove()
    {
        Vector3 playerPos = player.position;

        float left = currentCamPos.x - camWidth / 2f;
        float right = currentCamPos.x + camWidth / 2f;
        float bottom = currentCamPos.y - camHeight / 2f;
        float top = currentCamPos.y + camHeight / 2f;

        Vector3 targetPos = currentCamPos;

        //  좌우 이동
        if (playerPos.x > right)
            targetPos.x += camWidth;
        else if (playerPos.x < left)
            targetPos.x -= camWidth;

        // ▶ 위로만 이동 허용 (아래 X)
        if (playerPos.y > top)
            targetPos.y += camHeight;
        else if (playerPos.y < bottom)
        {
            // 아래로 이동하려는 경우 최소값 체크
            float nextY = targetPos.y - camHeight;
            if (nextY >= minY)
                targetPos.y = nextY;
        }

        if (targetPos != currentCamPos)
        {
            StartCoroutine(MoveCamera(currentCamPos, targetPos));
            currentCamPos = targetPos;
        }
    }

    IEnumerator MoveCamera(Vector3 start, Vector3 end)
    {
        isMoving = true;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float curveT = moveCurve.Evaluate(t);

            transform.position = Vector3.Lerp(start, end, curveT);
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}
