using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework;

public class HUDBehaviour : UIBehaviour {
    public Text globalMessage;

    //map
    public Text mapZoomTxt;
    private Camera m_BattleCam;

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
    public Button cameraFollowBtn;
    public Text cameraFollowTxt;

    //location
    public Button miniMapZoomBtn;
    public RectTransform miniMapPanel;

    private bool m_IsEnlarged;

    //Result
    public GameObject gameResultPanel;
    public Text gameResultTxt;

    //data
    private ShipData m_PlayerInitData;

    public override void Start() {
        base.Start();

        Game.Event.Subscribe("OnUpdateGlobalMessage", OnUpdateGlobalMessage);

        //map
        m_BattleCam = GameObject.FindGameObjectWithTag("BattleCam").GetComponent<Camera>();
        
        //status
        var pName=Game.Blackboard.GetData<string>("PlayerName");
        this.playerName.text= pName;
        
        var cName=Game.Blackboard.GetData<string>("ClubName");
        this.clubName.text= cName;
        
        //command
        sendScoutBtn.onClick.AddListener(OnSendScoutBtn);
        retrieveScoutBtn.onClick.AddListener(OnRetrieveScoutBtn);
        formationDropdown.onValueChanged.AddListener(OnFormationChanged);
        cameraFollowBtn.onClick.AddListener(OnCameraFollowBtn);

        //location
        miniMapZoomBtn.onClick.AddListener(OnMiniMapZoomBtn);

        //Game Result
        Game.Event.Subscribe("OnGameStart", OnGameStart);
        Game.Event.Subscribe("OnGameEnd", OnGameEnd);

        InitFormationOptions();
    }

    private void OnGameEnd(object sender, object e) {
        var ne = (bool) e;
        gameResultPanel.SetActive(true);
        gameResultTxt.text = ne ? "YOU   WIN" : "YOU   LOSE";
        //StartCoroutine(nameof(GameEndCo), ne);
    }


    private void OnGameStart(object sender, object e) {
        StartCoroutine(nameof(GameStartCo));
    }

    IEnumerator GameStartCo() {
        gameResultPanel.SetActive(true);
        gameResultTxt.text = "START";

        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime;
            yield return null;
        }

        gameResultPanel.SetActive(false);
        gameResultTxt.text = string.Empty;

        yield return null;
    }

    // IEnumerator GameEndCo(bool result) {
    //     gameResultPanel.SetActive(true);
    //     gameResultTxt.text = result ? "YOU   WIN" : "YOU   LOSE";
    //
    //     float t = 0f;
    //     while (t < 10f) {
    //         t += Time.deltaTime;
    //         yield return null;
    //     }
    //
    //     gameResultPanel.SetActive(false);
    //     gameResultTxt.text = string.Empty;
    //
    //     yield return null;
    // }

    private void Update() {
        UpdateStatus();

        if (m_PlayerInitData == null) {
            m_PlayerInitData = Game.Blackboard.GetData<ShipData>("PlayerInitialData");
        }

        var zoom = 5 / m_BattleCam.orthographicSize;
        mapZoomTxt.text = "Map   ( x " + zoom.ToString("0.0") + " )";
    }

    private void UpdateStatus() {
        var data = Game.Blackboard.GetData<ShipRuntimeData>("PlayerRuntimeData");
        if (data == null) {
            shipLeft.text = string.Empty;
            speed.text = string.Empty;
            offense.text = string.Empty;
            defense.text = string.Empty;
            return;
        }

        shipLeft.text = Mathf.Floor(data.hp) + "   /    " + Mathf.Floor(m_PlayerInitData.initialHp);
        speed.text = data.thrustLevel.ToString();
        offense.text = data.offense.ToString();
        defense.text = data.defense.ToString();
    }

    private void InitFormationOptions() {
        var options = new List<Dropdown.OptionData>();
        var data = Game.Blackboard.GetData<string[]>("StrategyNames");
        foreach (var option in data) {
            options.Add(new Dropdown.OptionData(option));
        }

        formationDropdown.options = options;
    }

    private void OnCameraFollowBtn() {
        var follow = Game.Blackboard.GetData<bool>("IsCameraFollow");
        Game.Blackboard.SetData("IsCameraFollow", !follow, BlackBoardDataType.Runtime);
        cameraFollowTxt.text = "视角追踪   " + (!follow ? "ON" : "OFF");
    }

    private void OnFormationChanged(int arg0) {
        Game.Event.Invoke("OnFormationUIValueChanged", null, arg0);
    }

    private void OnRetrieveScoutBtn() {
        var playerShip = GameManager.GetModule<ShipModule>().GetPlayerShip();
        var target = GameManager.GetModule<ShipModule>().GetClosestPlayerMiniShip(playerShip);
        Game.Event.Invoke("OnRetrievePlayerScout", null, target);
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

    private void OnMiniMapZoomBtn() {
        const float bigW = 678.51f;
        const float bigX = 620.76f;

        const float smallW = 381.4f;
        const float smallX = 769.31f;


        m_IsEnlarged = !m_IsEnlarged;

        var rect = miniMapPanel.rect;
        var w = m_IsEnlarged ? bigW : smallW;
        var x = m_IsEnlarged ? bigX : smallX;

        miniMapPanel.anchoredPosition = new Vector2(x, miniMapPanel.anchoredPosition.y);
        miniMapPanel.sizeDelta = new Vector2(w, miniMapPanel.sizeDelta.y);
    }
}