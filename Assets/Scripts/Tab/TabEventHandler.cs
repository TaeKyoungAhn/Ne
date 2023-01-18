using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TabEventHandler : MonoBehaviour
{

    public List<TabButton> tabButton;


    public TabButton selectedTab;
    public List<GameObject> panelToSwap; // 변경될 패널


    public void Subscribe(TabButton button)
    {
        if (tabButton == null)
        {
            tabButton = new List<TabButton>();
        }

        tabButton.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
              button.backGround.sprite = button.tabButtonHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {

        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
         button.backGround.sprite = button.tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < panelToSwap.Count; i++)
        {
            if (i == index)
            {
                panelToSwap[i].SetActive(true);
            }
            else
            {
                panelToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButton)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
             button.backGround.sprite = button.tabButtonIdle;
        }
    }
}
