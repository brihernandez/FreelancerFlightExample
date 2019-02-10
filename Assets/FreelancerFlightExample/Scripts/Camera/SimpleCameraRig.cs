using UnityEngine;

namespace FLFlight
{
    public class SimpleCameraRig : MonoBehaviour
    {
        [Tooltip("The ship to follow around.")]
        [SerializeField] private Transform ship = null;

        [Tooltip("Enable if the target to follow is being updated during FixedUpdate (e.g. if it is a Rigidbody using physics).")]
        [SerializeField] private bool useFixed = true;

        [Tooltip("How quickly the camera rotates to new positions. Tweak this values to get something that feels good. High values will result in tighter camera motion.")]
        [SerializeField] private float smoothSpeed = 10f;

        private void Update()
        {
            if (useFixed == false)
                MoveCamera();
        }

        private void FixedUpdate()
        {
            if (useFixed == true)
                MoveCamera();
        }

        private void MoveCamera()
        {
            if (ship == null)
                return;

            // Follow the ship around.
            transform.position = ship.position;

            // Using the look rotation of the ship's forward along with the rigs own up means
            // that the rig will follow the ship's rotation in pitch and yaw, but NOT in roll.
            // This allows the ship to roll on it own.
            var targetRigRotation = Quaternion.LookRotation(ship.forward, transform.up);
            transform.rotation = SmoothDamp.DampS(transform.rotation, targetRigRotation, smoothSpeed, Time.deltaTime);
        }
    }
}
