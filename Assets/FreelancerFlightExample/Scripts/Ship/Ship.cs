using UnityEngine;

namespace FLFlight
{
    /// <summary>
    /// Ties all the primary ship components together.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ShipPhysics))]
    [RequireComponent(typeof(ShipInput))]
    public class Ship : MonoBehaviour
    {
        [Tooltip("Set this ship to be the player ship. The player ship can always be accessed through the PlayerShip property.")]
        [SerializeField] private bool isPlayer = false;

        // Keep a static reference for whether or not this is the player ship. It can be used
        // by various gameplay mechanics. Returns the player ship if possible, otherwise null.
        public static Ship PlayerShip { get; private set; }

        public Vector3 Velocity { get { return Physics.Rigidbody.velocity; } }
        public ShipInput Input { get; private set; }
        public ShipPhysics Physics { get; internal set; }

        private void Awake()
        {
            Input = GetComponent<ShipInput>();
            Physics = GetComponent<ShipPhysics>();
        }

        private void Update()
        {
            // Pass the input to the physics to move the ship.
            Physics.SetPhysicsInput(new Vector3(Input.Strafe, 0.0f, Input.Throttle), new Vector3(Input.Pitch, Input.Yaw, Input.Roll));

            // If this is the player ship, then set the static reference. If more than one ship
            // is set to player, then whatever happens to be the last ship to be updated will be
            // considered the player. Don't let this happen.
            if (isPlayer)
                PlayerShip = this;
        }
    }
}
