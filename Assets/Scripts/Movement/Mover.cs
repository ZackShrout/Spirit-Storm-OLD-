using SpiritStorm.Core;
using SpiritStorm.Saving;
using UnityEngine;

namespace SpiritStorm.Movement
{
    public class Mover : MonoBehaviour, ISaveable, IAction
    {
        [SerializeField] Rigidbody2D theRB;
        private Animator myAnim;

        void Start()
        {
            myAnim = GetComponent<Animator>();
        }
        
        public void MoveTo(Vector2 direction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            theRB.velocity = new Vector2(direction.x, direction.y);
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            myAnim.SetFloat("moveX", theRB.velocity.x);
            myAnim.SetFloat("moveY", theRB.velocity.y);
            
            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        public void CancelAction()
        {
            MoveTo(Vector2.zero);
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
