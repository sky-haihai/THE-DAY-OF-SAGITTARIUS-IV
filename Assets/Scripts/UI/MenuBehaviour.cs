using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XiheFramework;

public class MenuBehaviour : UIBehaviour {
    public Button startGameBtn;
    public InputField inputField;

    public override void Start() {
        base.Start();

        startGameBtn.onClick.AddListener(OnStartGameBtn);
    }

    private void OnStartGameBtn() {
        Game.Blackboard.SetData("PlayerName", inputField.text, BlackBoardDataType.Runtime);
        Game.Blackboard.SetData("ClubName", "SOS団", BlackBoardDataType.Runtime);
        SceneManager.LoadScene("SampleStage");
    }
}