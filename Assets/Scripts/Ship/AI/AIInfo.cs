using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInfo {
    public AIMotherShip motherShip;
    public Formations formation;

    public AIInfo(AIMotherShip motherShip, Formations formation) {
        this.motherShip = motherShip;
        this.formation = formation;
    }

    public AIInfo() {
    }
}