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
    private string[] m_StrategyNames = null;

    public override void Setup() {
        Game.Blackboard.SetData("bound", bound, BlackBoardDataType.Runtime);

        m_StrategyNames = nameFormationPairs.Select(pair => pair.name).ToArray();
        Game.Blackboard.SetData("StrategyNames", m_StrategyNames, BlackBoardDataType.Runtime);

        m_AIBrain = new AIBrain();

        m_VisibilityTree = new MultiDictionary<int, ShipBase>();
    }

    public PlayerMotherShip GetPlayerShip() {
        return m_ShipList.OfType<PlayerMotherShip>().FirstOrDefault();
    }

    public IFormationStrategy GetStrategyById(int id) {
        return nameFormationPairs[id].strategy;
    }

    public ShipBase GuessBestTarget(ShipBase originShip) {
        ShipBase result = null;
        float highest = -10000f;
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

            const float ratio = 2f; //dist 1.7 : rot 1
            var distScore = -Mathf.Pow(delta.magnitude / 5f, 2.5f) * ratio + ratio;
            var rotScore = -Mathf.Pow(angle / 180f, 1.5f);
            var priScore = 3 * ship.shipData.priority;

            var s = distScore + rotScore + priScore;

            if (s > highest) {
                highest = s;
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

    public PlayerMiniShip GetClosestPlayerMiniShip(PlayerMotherShip motherShip) {
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

        return (PlayerMiniShip) result;
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

    public float ApplyAttack(ShipBase from, ShipBase to, out float multiplier) {
        float multi = 1f;
        var isFromMotherShip = from is PlayerMotherShip || from is AIMotherShip;
        var isToMotherShip = to is PlayerMotherShip || to is AIMotherShip;

        if (isFromMotherShip && isToMotherShip) {
            multi = 4f;
        }

        if (isFromMotherShip && !isToMotherShip) {
            //Debug.Log("mother -> mini");
            multi = 0.5f;
        }

        if (!isFromMotherShip && isToMotherShip) {
            multi = 1.5f;
        }

        if (!isFromMotherShip && !isToMotherShip) {
            multi = 1f;
        }

        var slope = from.shipData.offense / to.shipData.defense;
        const float n = 3f;
        var y = (1 - 1 / (slope + 1)) * n;
        var damage = y * 100f * multi * Time.deltaTime;
        to.ReceiveDamage(damage);

        multiplier = multi;
        return damage;
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

    public override void ShutDown(ShutDownType shutDownType) {
        m_ShipList.Clear();
        m_VisibilityTree.Clear();
        m_PlayerAutoLock = false;
    }

    public int GetPlayerMiniShipCountOf(PlayerMotherShip playerMotherShip) {
        int sum = 0;
        foreach (var shipBase in m_ShipList) {
            if (shipBase is PlayerMiniShip) {
                var ship = shipBase as PlayerMiniShip;
                if (ship.GetMotherShip() == playerMotherShip) {
                    sum += 1;
                }
            }
        }

        return sum;
    }

    public bool IsAnyEnemyShipAlive() {
        foreach (var shipBase in m_ShipList) {
            if (shipBase is AIMotherShip) {
                return true;
            }
        }

        return false;
    }
}

[Serializable]
public class NameStrategyPair {
    public string name;
    public StrategyBase strategy;
}