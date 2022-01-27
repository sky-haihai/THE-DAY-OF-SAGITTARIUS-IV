using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework;

public class PopupUI : UIBehaviour
{
    public GameObject popupPanel;
    
    public override void Active() {
        gameObject.SetActive(true);
    }

    public override void UnActive() {
        gameObject.SetActive(false);
    }
}
