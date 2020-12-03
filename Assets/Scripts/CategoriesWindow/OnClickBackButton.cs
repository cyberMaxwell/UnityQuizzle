using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickBackButton : MonoBehaviour
{   
    public GameObject mainPanel;
    public GameObject categoryPanel;

    public void OnClickBackButtonMethod()
    {
        categoryPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
