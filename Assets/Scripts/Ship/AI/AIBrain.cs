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
    private Dictionary<string, AIInfo> m_AiInfos;

    public float decisionTime = 0.1f;

    public AIBrain() {
        m_AiInfos = new Dictionary<string, AIInfo>();
    }

    public void Register(AIMotherShip motherShip, Formations defaultFormation) {
        var name = string.IsNullOrEmpty(motherShip.shipData.shipName) ? motherShip.GetHashCode().ToString() : motherShip.shipData.shipName;

        if (!m_AiInfos.ContainsKey(name)) {
            m_AiInfos.Add(name, new AIInfo(motherShip, defaultFormation));
        }
    }

    public void UnRegister(AIMotherShip motherShip) {
        var name = string.IsNullOrEmpty(motherShip.shipData.shipName) ? motherShip.GetHashCode().ToString() : motherShip.shipData.shipName;

        if (m_AiInfos.ContainsKey(name)) {
            m_AiInfos.Remove(name);
        }
    }

    void RefreshDecision() {
    }

    IFormationStrategy GetStrategyByName(Formations formation) {
        switch (formation) {
            case Formations.Spread:
                return new SpreadFormation();
            case Formations.Line:
                return new LineFormation();
            case Formations.Wedge:
                return new WedgeFormation();
            case Formations.Circular:
                return new CircularFormation();
            case Formations.BentLine:
                return new BentLineFormation();
            case Formations.Horseshoe:
                return new HorseshoeFormation();
            default:
                return new SpreadFormation();
        }
    }
}