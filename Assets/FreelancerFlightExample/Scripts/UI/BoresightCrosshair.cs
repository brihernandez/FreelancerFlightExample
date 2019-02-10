using UnityEngine;

namespace FLFlight.UI
{
    public class BoresightCrosshair : MonoBehaviour
    {
        public Transform ship;
        public float boresightDistance = 1000f;

        void Update()
        {
            if (ship != null)
            {
                Vector3 boresightPos = (ship.transform.forward * boresightDistance) + ship.transform.position;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(boresightPos);
                screenPos.z = 0f;

                transform.position = screenPos;
            }
        }
    }
}
