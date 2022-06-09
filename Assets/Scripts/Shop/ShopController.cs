using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<GameObject> backgrounds;

    [ValueDropdown("backgrounds")]
    [OnValueChanged("updateBackground")]
    public GameObject background;

    public List<GameObject> desks;

    [ValueDropdown("desks")]
    [OnValueChanged("updateDesk")]
    public GameObject desk;

    private void updateBackground()
    {
        foreach (GameObject go in backgrounds)
        {
            if (go != null)
            {
                go.SetActive(false);
            }
        }
        if (background != null)
        {
            background.SetActive(true);
        }
    }

    private void updateDesk()
    {
        foreach (GameObject go in desks)
        {
            if (go != null)
            {
                go.SetActive(false);
            }
        }
        if (desk != null)
        {
            desk.SetActive(true);
        }
    }
}
