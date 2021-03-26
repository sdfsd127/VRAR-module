using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool SHOW_DEBUGGING;

    private Shop shop;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        shop = GameObject.Find("Shop Panel").GetComponent<Shop>();
    }

    public static bool IsDebugMode()
    {
        if (Instance.SHOW_DEBUGGING)
            return true;
        else
            return false;
    }

    public bool IsShopOpen()
    {
        return shop.IsShopOpen();
    }
}
