using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework;

public class YUKIUI : UIBehaviour {
    public GameObject cmdWindow;

    public InputField inputField;

    public override void Start() {
        base.Start();

        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string text) {
        if (text.Equals("yuki.exe", StringComparison.OrdinalIgnoreCase)) {
            Game.Event.Invoke("YUKI", null, null);
        }
    }

    private void FixedUpdate() {
        if (Game.Input.GetKeyDown(KeyActionTypes.YUKI)) {
            cmdWindow.SetActive(!cmdWindow.activeSelf);
        }
    }

    public override void Active() {
        base.Active();
    }

    public override void UnActive() {
        base.UnActive();
    }
}