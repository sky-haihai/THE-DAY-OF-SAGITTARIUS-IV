using System;
using UnityEngine;
using UnityEngine.Rendering;
using XiheFramework;

[Serializable]
public abstract class ShipBase : MonoBehaviour {
    public ShipData shipData = new ShipData();

    public ShipBase target;
    public Transform fovSphere;
    public Renderer fovMaskRenderer;
    public Renderer fovObjectRenderer;

    private static readonly int StencilId = Shader.PropertyToID("_StencilID");
    private static readonly int Color = Shader.PropertyToID("_Color");

    //public abstract void TryLockTarget(Vector3 worldPosition);

    protected virtual void Start() {
        GameManager.GetModule<ShipModule>().Register(this);

        var radius = shipData.viewRadius * 2;
        fovSphere.localScale = new Vector3(radius, radius, radius);

        fovMaskRenderer.material.SetInt(StencilId, shipData.clubId);
        fovMaskRenderer.material.renderQueue = (int) RenderQueue.Geometry + shipData.clubId;

        fovObjectRenderer.material.SetInt(StencilId, shipData.clubId);
        fovObjectRenderer.material.SetColor(Color, shipData.clubColor);
        fovObjectRenderer.material.renderQueue = (int) RenderQueue.Geometry - shipData.clubId;
    }

    protected virtual void Update() {
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, shipData.viewRadius);
    }
}