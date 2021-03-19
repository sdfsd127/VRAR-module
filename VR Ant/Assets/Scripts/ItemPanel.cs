using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    private string m_Name;
    private float m_Price;

    public void SetupItem(string name, float price)
    {
        transform.GetChild(1).GetComponent<Text>().text = name;
        transform.GetChild(2).GetComponent<Text>().text = "£ " + price.ToString();
        transform.GetChild(3).GetComponent<Text>().text = "This is a " + name + ".";

        transform.GetChild(4).GetComponent<Button>().onClick.AddListener(OnClickEvent);

        m_Name = name;
        m_Price = price;
    }

    private void OnClickEvent()
    {
        Ant.Instance.PurchaseItem(m_Name, m_Price);
    }
}
