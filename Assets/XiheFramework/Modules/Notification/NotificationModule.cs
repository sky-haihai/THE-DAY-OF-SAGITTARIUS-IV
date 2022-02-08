
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework;

public class NotificationModule : GameModule
{
    public GameObject confirmCanvasPrefab;

    public GameObject popupCanvasPrefab;

    private PopupUI m_PopupUI;
    private ConfirmUI m_ConfirmUI;

    public void Popup(string context)
    {
        m_PopupUI = FindObjectOfType<PopupUI>();
        if (!m_PopupUI)
        {
            m_PopupUI=Instantiate(popupCanvasPrefab).GetComponent<PopupUI>();
        }
        m_PopupUI.popupPanel.SetActive(true);
        m_PopupUI.GetComponentInChildren<Text>().text = context;
    }

    public void Confirm(string context,string confirmEventName,string cancelEventName)
    {
        m_ConfirmUI = FindObjectOfType<ConfirmUI>();
        if (!m_ConfirmUI)
        {
            m_ConfirmUI=Instantiate(confirmCanvasPrefab).GetComponent<ConfirmUI>();
        }
        m_ConfirmUI.confirmPanel.SetActive(true);
        m_ConfirmUI.GetComponentInChildren<Text>().text= context;
        m_ConfirmUI.yesButton.onClick.AddListener(() =>
        {
            m_ConfirmUI.yesButton.onClick.RemoveAllListeners();
            Game.Event.Invoke(confirmEventName, m_ConfirmUI, null);
            m_ConfirmUI.confirmPanel.SetActive(false);
        });
        if (cancelEventName != null)
        {
            m_ConfirmUI.noButton.onClick.RemoveAllListeners();
            m_ConfirmUI.noButton.onClick.AddListener(() =>
            {
                Game.Event.Invoke(cancelEventName, m_ConfirmUI, null);
                m_ConfirmUI.confirmPanel.SetActive(false);
            });
        }
        else
        {
            m_ConfirmUI.noButton.onClick.RemoveAllListeners();
            m_ConfirmUI.noButton.onClick.AddListener(() =>
            {
                Debug.Log("m_ConfirmUI.noButton.onClick.AddListener");
                m_ConfirmUI.confirmPanel.SetActive(false);
            });
        }
    }
    void Start()
    {
        
    }
    
    public override void Update()
    {
        
    }

    public override void ShutDown(ShutDownType shutDownType)
    {
        
    }
}
