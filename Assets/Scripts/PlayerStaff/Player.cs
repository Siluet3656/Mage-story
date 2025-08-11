using UnityEngine;
using Data;

namespace PlayerStaff
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            G.Player = this;
        }
    }
}
