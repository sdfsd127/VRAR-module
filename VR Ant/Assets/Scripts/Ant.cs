using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public static Ant Instance;

    // Movement
    private Rigidbody rb;
    [SerializeField] private KeyCode[] MOVEMENT_KEYS; // Up, Down, Right, Left
    [SerializeField] [Range(1, 25000)] private float moveSpeed;

    // Money
    [SerializeField] [Range(0, 10000)] private float playerStartCurrency;
    private float playerCurrency;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();


        playerCurrency = playerStartCurrency;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(MOVEMENT_KEYS[0])) // Forward
        {
            rb.AddForce(transform.forward * moveSpeed);
        }

        if (Input.GetKey(MOVEMENT_KEYS[1])) // Left
        {
            rb.AddForce(-transform.right * moveSpeed);
        }

        if (Input.GetKey(MOVEMENT_KEYS[2])) // Back
        {
            rb.AddForce(-transform.forward * moveSpeed);
        }

        if (Input.GetKey(MOVEMENT_KEYS[3])) // Right
        {
            rb.AddForce(transform.right * moveSpeed);
        }
    }

    //
    // EXTERNAL CALLS
    //
    public bool HasEnoughMoney(float amount) { return (playerCurrency >= amount) ? true : false; }
}
