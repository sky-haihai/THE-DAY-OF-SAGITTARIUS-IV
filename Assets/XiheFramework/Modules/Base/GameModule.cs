using FlowCanvas.Nodes;
using UnityEngine;

namespace XiheFramework {
    public abstract class GameModule : MonoBehaviour {

        protected virtual void Awake() {
            GameManager.RegisterComponent(this);
        }

        public abstract void Update();

        public abstract void ShutDown();
    }
}