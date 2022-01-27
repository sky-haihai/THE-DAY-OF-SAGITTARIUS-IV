using System;
using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework {
    public class CameraModule : GameModule {
        private readonly List<CameraAgent> m_CameraAgents = new List<CameraAgent>();

        public void FocusTarget(Vector3 target)
        {
            m_CameraAgents[0].FocusTarget(target);
        }
        
        public void RegisterAgent(CameraAgent agent) {
            if (!m_CameraAgents.Contains(agent)) {
                m_CameraAgents.Add(agent);
            }
        }

        public float GetCameraSize()
        {
            return m_CameraAgents[0].GetCameraSize();
        }
        
        public override void Update() {
            foreach (var agent in m_CameraAgents) {
                agent.OnUpdate();
            }
        }

        public override void ShutDown() { }
    }
}