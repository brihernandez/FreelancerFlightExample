using UnityEngine;

namespace FLFlight
{
    /// <summary>
    /// Applies linear and angular forces to a ship.
    /// This is based on the ship physics from https://github.com/brihernandez/UnityCommon/blob/master/Assets/ShipPhysics/ShipPhysics.cs
    /// </summary>
    public class ShipPhysics : MonoBehaviour
    {
        [Tooltip("X: Lateral thrust\nY: Vertical thrust\nZ: Longitudinal Thrust")]
        public Vector3 linearForce = new Vector3(100.0f, 100.0f, 100.0f);

        [Tooltip("X: Pitch\nY: Yaw\nZ: Roll")]
        public Vector3 angularForce = new Vector3(100.0f, 100.0f, 100.0f);

        [Range(0.0f, 1.0f)]
        [Tooltip("Multiplier for longitudinal thrust when reverse thrust is requested.")]
        public float reverseMultiplier = 1.0f;

        [Tooltip("Multiplier for all forces. Can be used to keep force numbers smaller and more readable.")]
        public float forceMultiplier = 100.0f;

        /// <summary>
        /// Accessor for the Rigidbody controlling this ship.
        /// </summary>
        public Rigidbody Rigidbody { get; private set; }

        private Vector3 appliedLinearForce = Vector3.zero;
        private Vector3 appliedAngularForce = Vector3.zero;

        // Use this for initialization
        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null)
            {
                Debug.LogWarning(name + ": ShipPhysics has no rigidbody.");
            }
        }

        void FixedUpdate()
        {
            if (Rigidbody != null)
            {
                Rigidbody.AddRelativeForce(appliedLinearForce * forceMultiplier, ForceMode.Force);
                Rigidbody.AddRelativeTorque(appliedAngularForce * forceMultiplier, ForceMode.Force);
            }
        }

        /// <summary>
        /// Sets the input for how much of linearForce and angularForce are applied
        /// to the ship. Each component of the input vectors is assumed to be scaled
        /// from -1 to 1, but is not clamped.
        /// </summary>
        public void SetPhysicsInput(Vector3 linearInput, Vector3 angularInput)
        {
            appliedLinearForce = Vector3.Scale(linearForce, linearInput);
            appliedAngularForce = Vector3.Scale(angularForce, angularInput);
            //appliedLinearForce = MultiplyByComponent(linearInput, linearForce);
            //appliedAngularForce = MultiplyByComponent(angularInput, angularForce);
        }

        ///// <summary>
        ///// Returns a Vector3 where each component of Vector A is multiplied by the equivalent component of Vector B.
        ///// </summary>
        //private Vector3 MultiplyByComponent(Vector3 a, Vector3 b)
        //{
        //    a.

        //    Vector3 ret;

        //    ret.x = a.x * b.x;
        //    ret.y = a.y * b.y;
        //    ret.z = a.z * b.z;

        //    return ret;
        //}
    }
}
