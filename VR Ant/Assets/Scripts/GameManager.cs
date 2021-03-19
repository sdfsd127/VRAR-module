using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool SHOW_DEBUGGING;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public static bool IsDebugMode()
    {
        if (Instance.SHOW_DEBUGGING)
            return true;
        else
            return false;
    }
}
