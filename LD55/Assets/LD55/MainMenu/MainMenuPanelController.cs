using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuPanelController : MonoBehaviour
{
    public GameObject Panel;
    public List<Collider> ButtonsToDisable;
    public Button ClosePanelButton;


    private void Awake()
    {
        BaseAwake();
    }

    public void BaseAwake()
    {
        Panel.transform.rotation = Quaternion.Euler(0, 90, 0);
        ButtonsToDisable = GameObject.FindObjectsOfType<MainMenuDisableForPanels>().Select(s => s.GetComponent<Collider>()).ToList();
        ButtonsToDisable.Add(GetComponent<Collider>());
        ClosePanelButton.onClick.AddListener(CloseTab);

        CloseTab();
    }

    public virtual void CloseTab()
    {
        Panel.transform.rotation = Quaternion.Euler(0, 90, 0);
        foreach (var button in ButtonsToDisable)
        {
            button.GetComponent<Collider>().enabled = true;
        }
    }

    public virtual void OpenTab()
    {
        Panel.transform.rotation = Quaternion.identity;
        foreach (var button in ButtonsToDisable)
        {
            button.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnMouseUp()
    {
        OpenTab();
    }
}