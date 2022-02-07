using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework;

public class SplashScreenBehaviour : UIBehaviour {
    public float duration;
    public Text chn;
    public Text eng;

    private float m_Timer;

    private void Update() {
        var alpha = Mathf.Sin(m_Timer * Mathf.PI / duration);
        chn.color = new Color(1, 1, 1, alpha);
        eng.color = new Color(1, 1, 1, alpha);
        m_Timer += Time.deltaTime;
        m_Timer = Mathf.Clamp(m_Timer, 0f, duration);
    }
}