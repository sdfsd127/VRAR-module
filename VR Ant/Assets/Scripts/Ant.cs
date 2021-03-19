using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public static Ant Instance;

    // Movement
    private Rigidbody rb;
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float playerMaxMoveSpeed;

    // Money
    [SerializeField] private float playerStartCurrency;
    private float playerCurrentCurrency;

    // Grabbing and Moving
    private bool mandiblesClosed;
    private GameObject grabbedObject;

    //
    // INITIALISATION
    //
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

        playerCurrentCurrency = playerStartCurrency;
    }

    //
    // GAME LOOP
    //
    private void Update()
    {
        // Toggle mandibles -> Open/Closed
        if (Input.GetKeyUp(KeyCode.Space))
        {
            mandiblesClosed = !mandiblesClosed;

            if (mandiblesClosed) // Now closing mandibles
            {
                // Search a box in front of the Ant for objects to grab
                Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, new Vector3(0.5f, 0.5f, 0.5f));

                // Find the interactive colliders
                List<Collider> interactiveColliders = new List<Collider>();
                for (int i = 0; i < colliders.Length; i++)
                    if (colliders[i].tag == "Interactable")
                        interactiveColliders.Add(colliders[i]);

                // Find the closest object from those found within the box
                if (interactiveColliders.Count > 0)
                {
                    float smallestDistance = float.MaxValue;
                    int smallestIndex = -1;

                    for (int i = 0; i < interactiveColliders.Count; i++)
                    {
                        float dist = Vector3.Distance(transform.position, interactiveColliders[i].transform.position);

                        if (dist < smallestDistance)
                        {
                            smallestDistance = dist;
                            smallestIndex = i;
                        }
                    }

                    GameObject interactingObject = interactiveColliders[smallestIndex].gameObject;
                    PickupObject(interactingObject);
                }
            }
            else // Now openning mandibles
            {

            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            rb.velocity += (transform.forward * playerMoveSpeed);

        if (Input.GetKey(KeyCode.A))
            rb.velocity -= (transform.right * playerMoveSpeed);

        if (Input.GetKey(KeyCode.S))
            rb.velocity -= (transform.forward * playerMoveSpeed);

        if (Input.GetKey(KeyCode.D))
            rb.velocity += (transform.right * playerMoveSpeed);

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -playerMaxMoveSpeed, playerMaxMoveSpeed), 0, Mathf.Clamp(rb.velocity.z, -playerMaxMoveSpeed, playerMaxMoveSpeed));
    }

    private void PickupObject(GameObject gameObject)
    {
        grabbedObject = gameObject;
        grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        grabbedObject.transform.parent = transform;
    }

    private void DropObject()
    {
        if (grabbedObject == null)
            return;

        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.transform.parent = null;
    }

    private void ChangePlayerCurrency(float amount) 
    { 
        playerCurrentCurrency += amount; 
    }

    //
    // EXTERNAL CALLS
    //
    public bool HasEnoughMoney(float amount) { return (playerCurrentCurrency >= amount) ? true : false; }

    public void PurchaseItem(string itemName, float itemPrice)
    {
        if (HasEnoughMoney(itemPrice))
        {
            ChangePlayerCurrency(-itemPrice);
            Furniture.Instance.SpawnFurniture(itemName);
        }
    }

    //
    // GIZMOS
    //
    private void OnDrawGizmos()
    {
        if (GameManager.Instance != null && GameManager.IsDebugMode())
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(transform.position + transform.forward, Vector3.one);
        }
    }
}
