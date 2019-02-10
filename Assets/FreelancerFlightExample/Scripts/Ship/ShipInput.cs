using UnityEngine;

namespace FLFlight
{
    /// <summary>
    /// Class specifically to deal with input.
    /// </summary>
    public class ShipInput : MonoBehaviour
    {
        [Tooltip("How far the ship will bank when turning.")]
        [SerializeField] private float bankLimit = 35f;

        [Tooltip("Sensitivity in the pitch axis.\n\nIt's best to play with this value until you can get something the results in full input when at the edge of the screen.")]
        [SerializeField] private float pitchSensitivity = 2.5f;
        [Tooltip("Sensitivity in the yaw axis.\n\nIt's best to play with this value until you can get something the results in full input when at the edge of the screen.")]
        [SerializeField] private float yawSensitivity = 2.5f;
        [Tooltip("Sensitivity in the roll axis.\n\nTweak to make responsive enough.")]
        [SerializeField] private float rollSensitivity = 1f;

        [Range(-1, 1)]
        [SerializeField] private float pitch;
        [Range(-1, 1)]
        [SerializeField] private float yaw;
        [Range(-1, 1)]
        [SerializeField] private float roll;
        [Range(-1, 1)]
        [SerializeField] private float strafe;
        [Range(0, 1)]
        [SerializeField] private float throttle;

        // How quickly the throttle reacts to input.
        private const float THROTTLE_SPEED = 0.5f;

        public float Pitch { get { return pitch; } }
        public float Yaw { get { return yaw; } }
        public float Roll { get { return roll; } }
        public float Strafe { get { return strafe; } }
        public float Throttle { get { return throttle; } }

        private void Update()
        {
            strafe = Input.GetAxis("Horizontal");

            SetStickCommandsUsingAutopilot();

            UpdateMouseWheelThrottle();
            UpdateKeyboardThrottle(KeyCode.W, KeyCode.S);
        }

        private void SetStickCommandsUsingAutopilot()
        {
            // Project the position of the mouse on screen out to some distance.
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1000f;
            Vector3 gotoPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Use that world position under the mouse as a target point.
            TurnTowardsPoint(gotoPos);

            // Use the mouse to bank the ship some degrees based on the mouse position.
            BankShipRelativeToUpVector(mousePos, Camera.main.transform.up);
        }

        /// <summary>
        /// Ship will roll relative to the given up vector, modified by the mouse position.
        /// E.g. If the mouse is centered, the ship's up will match the up vector. If the mouse
        /// is at the right extreme of the screen, it will bank right bankLimit degrees relative
        /// to the given up vector.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="upVector"></param>
        private void BankShipRelativeToUpVector(Vector3 mousePos, Vector3 upVector)
        {
            // A PID Controller is HIGHLY recommended to get the most out of this.
            // The "sensitivity" values are poor substitutes for them, but they work well enough for
            // the sake of a demo. A tuned PID controller on the other hand will give extremely
            // accurate and responsive ship flight.
            // See https://en.wikipedia.org/wiki/PID_controller
            // And https://forum.unity.com/threads/pid-controller.68390/
            // Video showing the difference it makes: https://www.youtube.com/watch?v=ErPgBHwWffE

            // The ship is meant to bank left/right based on how much the mouse cursor is to the left
            // or right of the screen. E.g. Mouse at left edge of screen means the ship should try to
            // bank counterclockwise by bankLimit degrees. 

            // Figure out most position relative to center of screen.
            // 0 is center, 1 is right, -1 is left.
            float bankInfluence = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
            bankInfluence = Mathf.Clamp(bankInfluence, -1f, 1f);

            // Throttle modifies the bank angle so that when at idle, the ship just flatly yaws.
            bankInfluence *= throttle;
            float bankTarget = bankInfluence * bankLimit;

            // Here's the special sauce. Roll so that the bank target is reached relative to the
            // up of the camera.
            float bankError = Vector3.SignedAngle(transform.up, upVector, transform.forward);
            bankError = bankError - bankTarget;

            // Clamp this to prevent wild inputs.
            bankError = Mathf.Clamp(bankError * 0.1f, -1f, 1f);

            // Roll to minimze error.
            roll = bankError * rollSensitivity;
        }

        /// <summary>
        /// Pitches and yaws the ship to look at the passed in world position.
        /// </summary>
        /// <param name="gotoPos">World position to turn the ship towards.</param>
        private void TurnTowardsPoint(Vector3 gotoPos)
        {
            Vector3 localGotoPos = transform.InverseTransformVector(gotoPos - transform.position).normalized;

            // Note that you would want to use a PID controller for this to make it more responsive.
            pitch = Mathf.Clamp(-localGotoPos.y * pitchSensitivity, -1f, 1f);
            yaw = Mathf.Clamp(localGotoPos.x * yawSensitivity, -1f, 1f);
        }

        /// <summary>
        /// Uses R and F to raise and lower the throttle.
        /// </summary>
        private void UpdateKeyboardThrottle(KeyCode increaseKey, KeyCode decreaseKey)
        {
            float target = throttle;

            if (Input.GetKey(increaseKey))
                target = 1.0f;
            else if (Input.GetKey(decreaseKey))
                target = 0.0f;

            throttle = Mathf.MoveTowards(throttle, target, Time.deltaTime * THROTTLE_SPEED);
        }

        /// <summary>
        /// Uses the mouse wheel to control the throttle.
        /// </summary>
        private void UpdateMouseWheelThrottle()
        {
            throttle += Input.GetAxis("Mouse ScrollWheel");
            throttle = Mathf.Clamp(throttle, 0.0f, 1.0f);
        }
    }
}

