using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework;

public class BattleCameraController : MonoBehaviour {
    public float speed;
    public bool invert;

    private Vector4 bound;
    private Camera cam;

    private void Start() {
        bound = Game.Blackboard.GetData<Vector4>("bound");
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (Game.Input.GetMouse(2)) {
            var delta = Game.Input.GetMouseDeltaPosition().ToVector3(V2ToV3Type.XZ) * (Time.deltaTime * speed);

            if (invert) {
                transform.position -= delta;
            }
            else {
                transform.position += delta;
            }
        }

        if (transform.position.x < bound.x + cam.orthographicSize) {
            transform.position = new Vector3(bound.x + cam.orthographicSize, transform.position.y, transform.position.z);
        }

        if (transform.position.z < bound.y + cam.orthographicSize) {
            transform.position = new Vector3(transform.position.x, transform.position.y, bound.y + cam.orthographicSize);
        }

        if (transform.position.x > bound.z - cam.orthographicSize) {
            transform.position = new Vector3(bound.z - cam.orthographicSize, transform.position.y, transform.position.z);
        }

        if (transform.position.z > bound.w - cam.orthographicSize) {
            transform.position = new Vector3(transform.position.x, transform.position.y, bound.w - cam.orthographicSize);
        }
    }
}