using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public static Ant Instance;

    // Looking
    [SerializeField] private Camera antCam;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float minXLook, maxXLook;
    [SerializeField] private bool invertCameraXRot;
    private float curXRot;

    // Movement
    private Rigidbody rb;
    [SerializeField] private float playerMoveSpeed;

    // Money
    [SerializeField] private float playerStartCurrency;
    private float playerCurrentCurrency;

    // Grabbing and Moving
    private bool mandiblesClosed;
    private GameObject grabbedObject;
    private float grabbedObjectMass;

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

        CursorControl.SetCursorState(CursorLockMode.Confined, false);
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
                Debug.Log("Closing Mandibles");

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
                Debug.Log("Openning Mandibles");

                DropObject();
            }
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;
        dir.y = rb.velocity.y;

        rb.velocity = dir * playerMoveSpeed;
    }

    private void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += Vector3.up * x * lookSensitivity;

        if (invertCameraXRot)
            curXRot += y * lookSensitivity;
        else
            curXRot -= y * lookSensitivity;

        curXRot = Mathf.Clamp(curXRot, minXLook, maxXLook);

        Vector3 clampedAngle = antCam.transform.eulerAngles;
        clampedAngle.x = curXRot;
        antCam.transform.eulerAngles = clampedAngle;
    }

    private void PickupObject(GameObject gameObject)
    {
        grabbedObject = gameObject;
        SetPickupObjectState(false);
        grabbedObject.transform.parent.transform.parent = this.transform;

        Destroy(grabbedObject.GetComponent<Rigidbody>());
    }

    private void DropObject()
    {
        if (grabbedObject == null)
            return;

        grabbedObject.AddComponent<Rigidbody>().mass = grabbedObjectMass;

        SetPickupObjectState(true);
        grabbedObject.transform.parent.transform.parent = null;
        grabbedObject = null;
    }

    //
    // TOOLS
    //
    private void SetPickupObjectState(bool state)
    {
        Rigidbody grabbedObjectRB = grabbedObject.GetComponent<Rigidbody>();
        grabbedObjectRB.useGravity = state;
        grabbedObjectRB.detectCollisions = state;
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
