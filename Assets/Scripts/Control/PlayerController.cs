using SpiritStorm.Core;
using SpiritStorm.Movement;
using SpiritStorm.Saving;
using UnityEngine;

namespace SpiritStorm.Control
{
    public class PlayerController : MonoBehaviour, ISaveable
    {
        [SerializeField] Rigidbody2D theRB;
        [SerializeField] float moveSpeed;
        [SerializeField] bool wasMovingVertical = false;
        [SerializeField] float xPad = .25f, yPad = .5f;
        [SerializeField] Vector3 bottomLeftLimit, topRightLimit;
        [SerializeField] bool canMove = true;
        private FollowCam theScreen;

        void Start()
        {
            theScreen = FindObjectOfType<FollowCam>();
        }

        void Update()
        {
            MovePlayer();
            ClampPlayer();
        }

        private void MovePlayer()
        {
            if (canMove)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                bool isMovingHorizontal = Mathf.Abs(horizontal) > .5f;

                float vertical = Input.GetAxisRaw("Vertical");
                bool isMovingVertical = Mathf.Abs(vertical) > .5f;

                if (isMovingVertical && isMovingHorizontal)
                {
                    if (wasMovingVertical)
                    {
                        GetComponent<Mover>().MoveTo(new Vector2(horizontal * moveSpeed, 0f));
                    }
                    else
                    {
                        GetComponent<Mover>().MoveTo(new Vector2(0f, vertical * moveSpeed));
                    }
                }
                else if (isMovingHorizontal)
                {
                    GetComponent<Mover>().MoveTo(new Vector2(horizontal * moveSpeed, 0f));
                    wasMovingVertical = false;
                }
                else if (isMovingVertical)
                {
                    GetComponent<Mover>().MoveTo(new Vector2(0f, vertical * moveSpeed));
                    wasMovingVertical = true;
                }
                else
                {
                    GetComponent<Mover>().MoveTo(Vector2.zero);
                }
            }
            else
            {
                GetComponent<Mover>().MoveTo(Vector2.zero);
            }
        }

        private void ClampPlayer()
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, theScreen.GetBottomLeft().x + xPad, theScreen.GetTopRight().x - xPad),
                                             Mathf.Clamp(transform.position.y, theScreen.GetBottomLeft().y + yPad, theScreen.GetTopRight().y - yPad),
                                             transform.position.z);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            transform.position = position.ToVector();
        }
    }
}
