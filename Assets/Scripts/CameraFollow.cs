using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // 따라갈 플레이어
    public float smoothSpeed = 5f;  // 따라오는 속도
    public Vector3 offset = new Vector3(0, 0, -10);  // Z축 고정

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = new Vector3(
            target.position.x,
            target.position.y,
            offset.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}