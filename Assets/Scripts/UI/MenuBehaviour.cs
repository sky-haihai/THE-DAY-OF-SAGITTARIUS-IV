using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XiheFramework;

public class MenuBehaviour : UIBehaviour {
    public GameObject clearedIcon;
    public Button startGameBtn;
    public InputField inputField;

    public override void Start() {
        base.Start();

        startGameBtn.onClick.AddListener(OnStartGameBtn);

        var cleared = Game.Blackboard.GetData<bool>("SampleStageCleared");
        if (cleared) {
            clearedIcon.SetActive(true);
        }
        else {
            clearedIcon.SetActive(false);
        }
    }

    private void OnStartGameBtn() {
        Game.Blackboard.SetData("PlayerName", inputField.text, BlackBoardDataType.Runtime);
        Game.Blackboard.SetData("ClubName", "SOS団", BlackBoardDataType.Runtime);
        SceneManager.LoadScene("SampleStage");
    }
}