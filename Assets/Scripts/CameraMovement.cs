using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public Transform target;
    public float smoothing;

    public Vector2 minPos, maxPos;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void LateUpdate() {
        if (transform.position.x != target.position.x
        || transform.position.y != target.position.y) {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
    }
}
