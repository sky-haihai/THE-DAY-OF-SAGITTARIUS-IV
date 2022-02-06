using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using XiheFramework;
using XiheFramework.Util;

public class ShipModule : GameModule {
    private List<ShipBase> m_ShipList = new List<ShipBase>();
    private AIBrain m_AIBrain;

    private MultiDictionary<int, ShipBase> m_VisibilityTree; //club id, visible ships

    public List<NameStrategyPair> nameFormationPairs = new List<NameStrategyPair>();

    public Vector4 bound;

    private bool m_PlayerAutoLock = false;

    public override void Setup() {
        Game.Blackboard.SetData("bound", bound, BlackBoardDataType.Runtime);

        var names = nameFormationPairs.Select(pair => pair.name).ToArray();
        Game.Blackboard.SetData("StrategyNames", names, BlackBoardDataType.Runtime);

        m_AIBrain = new AIBrain();

        m_VisibilityTree = new MultiDictionary<int, ShipBase>();

        // m_DamageDisplayStructs = new List<DamageDisplayStruct>();

        //InitFormationOptions();
    }

    // private void InitFormationOptions() {
    //     var formationOptions = Enum.GetValues(typeof(Formations));
    //     List<string> result = new List<string>();
    //     foreach (var option in formationOptions) {
    //         result.Add(option.ToString());
    //     }
    //
    //     //formationOptions = (string[]) formationOptions;
    //     Game.Blackboard.SetData("FormationOptions", result.ToArray(), BlackBoardDataType.Runtime);
    // }

    public ShipBase GetPlayerShip() {
        return m_ShipList.OfType<PlayerMotherShip>().FirstOrDefault();
    }

    public IFormationStrategy GetStrategyById(int id) {
        return nameFormationPairs[id].strategy;
    }

    public ShipBase GuessBestTarget(ShipBase originShip) {
        ShipBase result = null;
        var smallest = 180f;
        if (!m_VisibilityTree.ContainsKey(originShip.ClubId)) {
            return null;
        }

        var candidates = m_VisibilityTree[originShip.ClubId];
        foreach (var ship in candidates) {
            var origin = originShip.transform;
            var delta = ship.transform.position - origin.position;
            delta.y = 0;

            //angle
            var angle = Vector3.Angle(origin.forward, delta);
            if (angle < smallest) {
                smallest = angle;
                result = ship;
            }
        }

        return result;
    }

    public ShipBase[] GetAllShipsOwnedBy(string owner) {
        var result = new List<ShipBase>();
        foreach (var shipBase in m_ShipList) {
            if (shipBase.shipData.shipOwner.Equals(owner)) {
                result.Add(shipBase);
            }
        }

        return result.ToArray();
    }

    public ShipBase GetClosestPlayerMiniShip(PlayerMotherShip motherShip) {
        ShipBase result = null;
        float dist = float.MaxValue;
        foreach (var shipBase in m_ShipList) {
            if (shipBase is PlayerMiniShip) {
                var s = (PlayerMiniShip) shipBase;
                var delta = Vector3.Distance(s.GetMotherShip().transform.position, shipBase.transform.position);
                if (delta < dist) {
                    dist = delta;
                    result = shipBase;
                }
            }
        }

        return result;
    }

    public float GetShipLeftOwnedBy(string owner) {
        float sum = 0f;
        foreach (var shipBase in m_ShipList) {
            if (shipBase.shipData.shipOwner.Equals(owner)) {
                sum += shipBase.runtimeData.hp;
            }
        }

        return sum;
    }

    public void ApplyAttack(ShipBase from, ShipBase to) {
        var damage = from.shipData.offense / to.shipData.defense * Time.deltaTime;
        to.ReceiveDamage(damage);

        //m_DamageDisplayStructs.Add(new DamageDisplayStruct(from.transform.position, to.transform.position));
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

    public void RegisterAI(AIMotherShip ai) {
        m_AIBrain.Register(ai);
    }

    public override void Update() {
        if (Game.Input.GetKeyDown(KeyActionTypes.AutoLock)) {
            m_PlayerAutoLock = !m_PlayerAutoLock;
            Game.Event.Invoke("OnAutoLock", this, m_PlayerAutoLock);
        }

        UpdateVisibility();
    }

    private void UpdateVisibility() {
        m_VisibilityTree.Clear();

        foreach (var ship in m_ShipList) {
            AddViewableShips(ship, false);
        }
    }

    //TODO: wrong, fix
    private ShipBase[] GetViewableShips(int clubId) {
        return m_VisibilityTree[clubId].ToArray();
    }

    private void AddViewableShips(ShipBase originShip, bool friendlyInclusive) {
        foreach (var ship in m_ShipList) {
            if (m_VisibilityTree.ContainsKey(originShip.ClubId)) {
                if (m_VisibilityTree.ContainsValue(originShip.ClubId, ship)) {
                    continue;
                }
            }

            if (!friendlyInclusive && ship.ClubId == originShip.ClubId) {
                continue;
            }

            var delta = ship.transform.position - originShip.transform.position;
            delta.y = 0;
            if (delta.magnitude < originShip.shipData.viewRadius) {
                m_VisibilityTree.Add(originShip.ClubId, ship);
            }
        }
    }

    public override void ShutDown() {
        m_ShipList.Clear();
    }
}

[Serializable]
public class NameStrategyPair {
    public string name;
    public StrategyBase strategy;
}