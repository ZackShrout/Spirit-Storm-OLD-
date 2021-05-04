using UnityEngine;
using UnityEngine.Tilemaps;

namespace SpiritStorm.Core
{
    public class FollowCam : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Tilemap theMap;
        private Vector3 bottomLeftLimit, topRightLimit;

        private float halfHeight, halfWidth;

        void Start()
        {
            halfHeight = Camera.main.orthographicSize;
            halfWidth = halfHeight * Camera.main.aspect;

            bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
            topRightLimit = theMap.localBounds.max - new Vector3(halfWidth, halfHeight, 0);
        }

        void Update()
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
        }

        public Vector3 GetBottomLeft()
        {
            return theMap.localBounds.min;
        }

        public Vector3 GetTopRight()
        {
            return theMap.localBounds.max;
        }
    }
}
