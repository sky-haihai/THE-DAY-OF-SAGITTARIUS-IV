using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework;

public class HUDBehaviour : UIBehaviour {
    public Text globalMessage;

    //status
    public Text clubName;
    public Text playerName;
    public Text shipLeft;
    public Text speed;
    public Text offense;
    public Text defense;

    //condition

    //command

    //location

    public override void Start() {
        base.Start();

        Game.Event.Subscribe("OnUpdateGlobalMessage", OnUpdateGlobalMessage);
    }

    private void OnUpdateGlobalMessage(object sender, object e) {
        SetGlobalMessage((string) e);
    }

    private void SetGlobalMessage(string message) {
        globalMessage.text = message;
    }
}