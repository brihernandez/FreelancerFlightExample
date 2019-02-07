using UnityEngine;

/// <summary>
/// Ties all the primary ship components together.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShipPhysics))]
[RequireComponent(typeof(ShipInput))]
public class Ship : MonoBehaviour
{    
    public bool isPlayer = false;

    public ShipInput Input { get; private set; }
    private ShipPhysics physics;    

    // Keep a static reference for whether or not this is the player ship. It can be used
    // by various gameplay mechanics. Returns the player ship if possible, otherwise null.
    public static Ship PlayerShip { get; private set; }

    // Getters for external objects to reference things like input.
    public Vector3 Velocity { get { return physics.Rigidbody.velocity; } }

    private void Awake()
    {
        Input = GetComponent<ShipInput>();
        physics = GetComponent<ShipPhysics>();
    }

    private void Update()
    {
        // Pass the input to the physics to move the ship.
        physics.SetPhysicsInput(new Vector3(Input.Strafe, 0.0f, Input.Throttle), new Vector3(Input.Pitch, Input.Yaw, Input.Roll));

        // If this is the player ship, then set the static reference. If more than one ship
        // is set to player, then whatever happens to be the last ship to be updated will be
        // considered the player. Don't let this happen.
        if (isPlayer)
            PlayerShip = this;
    }
}
