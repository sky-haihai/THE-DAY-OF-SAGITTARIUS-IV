using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework;

public class BattleCameraController : MonoBehaviour {
    public float moveSpeed;
    public bool invertMove;
    public float zoomSpeed;
    public bool invertZoom;

    public bool followPlayer;

    private Vector4 bound;
    private Camera cam;

    private Vector3 m_Dest;

    private void Start() {
        bound = Game.Blackboard.GetData<Vector4>("bound");
        cam = GetComponent<Camera>();
    }

    private void Update() {
        HandleInput();

        HandleMove();

        HandleScale();

        ClampPos();
    }

    private void HandleInput() {
        if (followPlayer) {
            m_Dest = GameManager.GetModule<ShipModule>().GetPlayerShip().transform.position;
            return;
        }

        if (Game.Input.GetMouse(2)) {
            var delta = Game.Input.GetMouseDeltaPosition().ToVector3(V2ToV3Type.XZ) * (Time.deltaTime * moveSpeed);
            delta *= cam.orthographicSize / 5;
            delta = Vector3.ClampMagnitude(delta, 1f);
            if (invertMove) {
                m_Dest -= delta;
            }
            else {
                m_Dest += delta;
            }
        }
    }

    private void HandleMove() {
        m_Dest.y = 5f;
        transform.position = Vector3.Lerp(transform.position, m_Dest, Time.deltaTime * 12f);
    }

    private void HandleScale() {
        var delta = Input.GetAxis("Mouse ScrollWheel");
        delta = delta * Time.deltaTime * zoomSpeed * 100f;
        delta = invertZoom ? delta : -delta;
        var target = cam.orthographicSize + delta;
        target = Mathf.Clamp(target, 5f, Mathf.Min(bound.z, bound.w) / 2);

        cam.orthographicSize = target;
    }

    void ClampPos() {
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