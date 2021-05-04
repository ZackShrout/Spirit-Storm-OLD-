using UnityEngine;

namespace SpiritStorm.Core
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] float timePlayed;

        void Update()
        {
            timePlayed += Time.deltaTime;
        }

        public float GetTimePlayed()
        {
            return timePlayed;
        }
    }
}
