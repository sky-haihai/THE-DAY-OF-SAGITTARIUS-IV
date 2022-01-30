using System;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework;

public class ShipModule : GameModule {
    private List<ShipBase> m_ShipList = new List<ShipBase>();

    private ShipComputeData[] m_ShipComputeData;

    public Vector4 bound;

    private bool m_PlayerAutoLock = false;

    public override void Setup() {
        Game.Blackboard.SetData("bound", bound, BlackBoardDataType.Runtime);
    }

    public void Register(ShipBase shipBase) {
        if (!m_ShipList.Contains(shipBase)) {
            m_ShipList.Add(shipBase);
        }
    }

    public void UnRegister(ShipBase shipBase) {
        if (m_ShipList.Contains(shipBase)) {
            m_ShipList.Remove(shipBase);
        }
    }

    public ShipComputeData[] UpdateComputeData() {
        m_ShipComputeData = new ShipComputeData[m_ShipList.Count];
        for (int i = 0; i < m_ShipComputeData.Length; i++) {
            var ship = m_ShipList[i];
            m_ShipComputeData[i] = new ShipComputeData(ship.transform.position, ship.shipData.viewRadius);
        }

        return m_ShipComputeData;
    }

    public ShipComputeData[] GetShipComputeData() {
        return m_ShipComputeData;
    }

    public override void Update() {
        if (Game.Input.GetKeyDown(KeyActionTypes.AutoLock)) {
            m_PlayerAutoLock = !m_PlayerAutoLock;
            Game.Event.Invoke("OnAutoLock", this, m_PlayerAutoLock);
        }
    }

    public override void ShutDown() {
        m_ShipList.Clear();
    }
}