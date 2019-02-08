using UnityEngine;
using UnityEngine.UI;

namespace FLFlight.UI
{
    /// <summary>
    /// Shows throttle and speed of the player ship.
    /// </summary>
    public class InputUI : MonoBehaviour
    {
        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (text != null && Ship.PlayerShip != null)
            {
                text.text = string.Format("{0:0.00}\n{1:0.00}\n{2:0.00}",
                                          Ship.PlayerShip.Input.Pitch,
                                          Ship.PlayerShip.Input.Yaw,
                                          Ship.PlayerShip.Input.Roll);
            }
        }
    }
}
