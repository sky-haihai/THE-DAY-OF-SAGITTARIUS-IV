using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework;

public static class ShipSvc {
    public static bool IsAnyEnemyAlive() {
        return GameManager.GetModule<ShipModule>().IsAnyEnemyShipAlive();
    }
}