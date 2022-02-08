using System;

[Serializable]
public class ShipRuntimeData {
    public string shipName;
    public float hp;
    public bool isDead;
    public float thrustLevel;
    public float offense;
    public float defense;
    public float moveSpeed;

    public ShipRuntimeData(string shipName, float hp, float offense, float defense, float moveSpeed) {
        this.shipName = shipName;
        this.hp = hp;
        this.isDead = false;
        this.thrustLevel = 0f;
        this.offense = offense;
        this.defense = defense;
        this.moveSpeed = moveSpeed;
    }

    public ShipRuntimeData(string shipName, float hp, float thrustLevel, float offense, float defense, float moveSpeed) {
        this.shipName = shipName;
        this.hp = hp;
        this.thrustLevel = thrustLevel;
        this.offense = offense;
        this.defense = defense;
        this.moveSpeed = moveSpeed;

        isDead = false;
    }
}