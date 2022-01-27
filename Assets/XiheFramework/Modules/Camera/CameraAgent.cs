using UnityEngine;

namespace XiheFramework {
    public abstract class CameraAgent:MonoBehaviour
    {

        public abstract void FocusTarget(Vector3 target);
        public abstract void OnUpdate();

        public abstract float GetCameraSize();
    }
}