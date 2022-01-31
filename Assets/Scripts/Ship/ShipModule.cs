using System;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework;

public class ShipModule : GameModule {
    private List<ShipBase> m_ShipList = new List<ShipBase>();
    private AIBrain m_AIBrain;

    public Vector4 bound;

    private bool m_PlayerAutoLock = false;

    private List<DamageDisplayStruct> m_DamageDisplayStructs;

    // private ShipComputeData[] m_ShipComputeData;

    public override void Setup() {
        Game.Blackboard.SetData("bound", bound, BlackBoardDataType.Runtime);

        m_AIBrain = new AIBrain();

        m_DamageDisplayStructs = new List<DamageDisplayStruct>();
    }

    /// <summary>
    /// generate a new mini ship from mother ship
    /// </summary>
    public void RequestPlayerMiniShip() {
        //init new player mini ship
    }

    public ShipBase[] GetViewableShips(ShipBase originShip, float radius) {
        var result = new List<ShipBase>();
        foreach (var ship in m_ShipList) {
            var delta = ship.transform.position - originShip.transform.position;
            delta.y = 0;
            if (delta.magnitude < radius) {
                result.Add(ship);
            }
        }

        return result.ToArray();
    }

    //return the ship within attack range which have least angle between the delta and transform.forward
    public ShipBase GuessBestTarget(ShipBase originShip, float radius) {
        ShipBase result = null;
        var smallest = 180f;
        foreach (var ship in m_ShipList) {
            if (ship == originShip) {
                continue;
            }

            var delta = ship.transform.position - originShip.transform.position;
            delta.y = 0;
            if (delta.magnitude < radius) {
                //angle
                var angle = Vector3.Angle(originShip.transform.forward, delta);
                if (angle < smallest) {
                    smallest = angle;
                    result = ship;
                }
            }
        }

        return result;
    }

    public void ApplyAttack(ShipBase from, ShipBase to) {
        var damage = from.shipData.offense / to.shipData.defense * Time.deltaTime;
        to.ReceiveDamage(damage);

        m_DamageDisplayStructs.Add(new DamageDisplayStruct(from.transform.position, to.transform.position));
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

    public void RegisterAI(AIMotherShip ai, Formations defaultStrategy) {
        m_AIBrain.Register(ai, defaultStrategy);
    }

    // public ShipComputeData[] UpdateComputeData() {
    //     m_ShipComputeData = new ShipComputeData[m_ShipList.Count];
    //     for (int i = 0; i < m_ShipComputeData.Length; i++) {
    //         var ship = m_ShipList[i];
    //         m_ShipComputeData[i] = new ShipComputeData(ship.transform.position, ship.shipData.viewRadius);
    //     }
    //
    //     return m_ShipComputeData;
    // }
    //
    // public ShipComputeData[] GetShipComputeData() {
    //     return m_ShipComputeData;
    // }

    public override void Update() {
        if (Game.Input.GetKeyDown(KeyActionTypes.AutoLock)) {
            m_PlayerAutoLock = !m_PlayerAutoLock;
            Game.Event.Invoke("OnAutoLock", this, m_PlayerAutoLock);
        }

        DisplayDamageData();
    }

    private void DisplayDamageData() {
        Game.Blackboard.SetData("DamageDisplayData", m_DamageDisplayStructs.ToArray(), BlackBoardDataType.Runtime);
        m_DamageDisplayStructs.Clear();
    }

    public override void ShutDown() {
        m_ShipList.Clear();
    }
}