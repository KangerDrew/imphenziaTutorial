using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // This will retrive the transform (rotation, position, scale) of a
    // specified model in the game:
    [SerializeField] private Transform groundCheckTransform = null;
    // Returns a list/array of layers of colliders, specified in the unity
    // (should be right below Ground Check Transform):
    [SerializeField] private LayerMask playerMask;

    // Unexposed fields:
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private Rigidbody rigidbodyComponent;
    private int superJumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component of the player model. This is so
        // that everytime we need to do something with Rigidbody, we
        // don't call it over and over again and needlessly making unity
        // look for the reference. We instead call it just once:
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if space key is pressed down:
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once every physics update:
    private void FixedUpdate()
    {
        // Debugging here:
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    foreach (var item in Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask))
        //    {
        //        Debug.Log(item.ToString());
        //    }

        //    if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        //    {
        //        Debug.Log("Nothing in the overlaph!");
        //    }
        //}

        // Takes the horizontalInput float value, and apply it to the model's Vector3.x:
        rigidbodyComponent.velocity = new Vector3(horizontalInput * 2, rigidbodyComponent.velocity.y, 0);

        // Generate a sphere at transform's position (of size 0.1), and see how many colliders it
        // overlaps. When player model is in the air, there should be only 1 (groundCheckTransform
        // is "colliding" with the player transform). When player is on the ground .Length value should
        // be 2 since it's also colliding with the floor:
        //if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f).Length == 1)
        //{
        //    return;
        //}

        // Alternate method for preventing player from jumping midair - Use LayerMask
        // collision detection:
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        // Applies jump motion to Player model, then re-sets jump key
        // boolean to false (so it doesn't fly away):
        if (jumpKeyWasPressed)
        {
            float jumpPower = 5f;
            if (superJumpsRemaining > 0)
            {
                jumpPower += 2;
                superJumpsRemaining--;
            }
            rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpKeyWasPressed = false;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
            superJumpsRemaining += 1;
        }
    }

}
