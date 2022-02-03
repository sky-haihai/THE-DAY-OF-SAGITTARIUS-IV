using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// inch-by-inch search stretegy
/// 1. enemy ships patrol and keep distance of each other, distance = a bit larger than the sum of two closest enemy ship
/// 2. 
/// </summary>
[Serializable]
public class AIBrain {
    private Dictionary<string, AIMotherShip> m_AiMotherShips;

    public float decisionTime = 0.1f;

    public AIBrain() {
        m_AiMotherShips = new Dictionary<string, AIMotherShip>();
    }

    public void Register(AIMotherShip motherShip) {
        var name = string.IsNullOrEmpty(motherShip.shipData.shipName) ? motherShip.GetHashCode().ToString() : motherShip.shipData.shipName;

        if (!m_AiMotherShips.ContainsKey(name)) {
            m_AiMotherShips.Add(name, motherShip);
        }
    }

    public void UnRegister(AIMotherShip motherShip) {
        var name = string.IsNullOrEmpty(motherShip.shipData.shipName) ? motherShip.GetHashCode().ToString() : motherShip.shipData.shipName;

        if (m_AiMotherShips.ContainsKey(name)) {
            m_AiMotherShips.Remove(name);
        }
    }

    void RefreshDecision() {
        
    }
}