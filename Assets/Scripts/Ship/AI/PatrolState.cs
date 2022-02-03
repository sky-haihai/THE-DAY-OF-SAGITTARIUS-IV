using XiheFramework;

public class PatrolState : State<AIMotherShip> {
    public PatrolState(StateMachine parentStateMachine, AIMotherShip owner) : base(parentStateMachine, owner) {
    }

    public override void OnEnter() {
    }

    public override void OnUpdate() {
        if (Owner.HasTarget()) {
            ChangeState(nameof(BattleState));
        }
    }

    public override void OnExit() {
        
    }
}