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
    public Button sendScoutBtn;
    public Button retrieveScoutBtn;
    public Dropdown formationDropdown;

    //location

    public override void Start() {
        base.Start();

        Game.Event.Subscribe("OnUpdateGlobalMessage", OnUpdateGlobalMessage);

        sendScoutBtn.onClick.AddListener(OnSendScoutBtn);
        sendScoutBtn.onClick.AddListener(OnRetrieveScoutBtn);
        formationDropdown.onValueChanged.AddListener(OnFormationChanged);
        InitFormationOptions();
    }

    private void InitFormationOptions() {
        var options = new List<Dropdown.OptionData>();
        var data = Game.Blackboard.GetData<string[]>("FormationOptions");
        foreach (var option in data) {
            options.Add(new Dropdown.OptionData(option));
        }

        formationDropdown.options = options;
    }

    private void OnFormationChanged(int arg0) {
        Game.Event.Invoke("OnFormationUIValueChanged", null, arg0);
    }

    private void OnRetrieveScoutBtn() {
        //TODO: implement
    }

    private void OnSendScoutBtn() {
        Game.Event.Invoke("OnSendPlayerScout", null, null);
    }

    private void OnUpdateGlobalMessage(object sender, object e) {
        SetGlobalMessage((string) e);
    }

    private void SetGlobalMessage(string message) {
        globalMessage.text = message;
    }
}