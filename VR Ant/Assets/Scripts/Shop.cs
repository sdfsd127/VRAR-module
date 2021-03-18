using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject[] tabPanels;

    [SerializeField] private GameObject shopItemPanelPrefab;
    private List<GameObject> shopItemList;

    [SerializeField] private KeyCode SHOPHOTKEY;

    [SerializeField] private bool RESET_SHOP_ON_CLOSE;

    private void Start()
    {
        ResetAllPanels();

        SetupItemShop();
    }

    private void Update()
    {
        if (Input.GetKeyUp(SHOPHOTKEY))
        {
            mainPanel.SetActive(!mainPanel.activeSelf);
            if (RESET_SHOP_ON_CLOSE)
                ResetAllPanels();
        }
    }

    //
    // Panels and Tabs
    //
    private void ResetAllPanels()
    {
        for (int i = 0; i < tabPanels.Length; i++)
            tabPanels[i].SetActive(false);
    }

    private void SetActiveTab(int tabID)
    {
        ResetAllPanels();

        tabPanels[tabID].SetActive(true);
    }

    //
    // Items in Shop
    //
    private void SetupItemShop()
    {
        FurnitureObject[] furniture = Furniture.Instance.GetAllFurniture();

        for (int i = 0; i < furniture.Length; i++)
        {
            FurnitureObject currentFurniture = furniture[i];
            int tabID = GetRelevantTab(currentFurniture.room);

            GameObject newItem = Instantiate(shopItemPanelPrefab, tabPanels[tabID].transform);
            newItem.GetComponent<ItemPanel>().SetupItem(currentFurniture.name, currentFurniture.price);
        }
    }

    private int GetRelevantTab(ROOM room)
    {
        switch (room)
        {
            case ROOM.Bathroom:
                return 0;
            case ROOM.Bedroom:
                return 1;
            case ROOM.Kitchen:
                return 2;
            case ROOM.LivingRoom:
                return 3;
            default:
                return -1;
        }
    }

    //
    // External Button Presses
    //
    public void TabButtonPressed(int tabID)
    {
        SetActiveTab(tabID);
    }

    public void PurchaseButtonPressed(string key)
    {
        Furniture.Instance.SpawnFurniture(key);
    }
}