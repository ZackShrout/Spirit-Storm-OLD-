using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpiritStorm.Saving;

public class PlayerController2 : MonoBehaviour, ISaveable
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    private bool wasMovingVertical = false;

    public Animator myAnim;

    public static PlayerController2 instance;

    public string areaTransitionName;

    private Vector3 bottomLeftLimit, topRightLimit;

    public bool canMove = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AnimatePlayer();
        ClampPlayer();
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.25f, .5f, 0);
        topRightLimit = topRight - new Vector3(.25f, .5f, 0);
    }

    private void MovePlayer()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            bool isMovingHorizontal = Mathf.Abs(horizontal) > .5f;

            float vertical = Input.GetAxisRaw("Vertical");
            bool isMovingVertical = Mathf.Abs(vertical) > .5f;

            if(isMovingVertical && isMovingHorizontal)
            {
                if (wasMovingVertical)
                {
                    theRB.velocity = new Vector2(horizontal * moveSpeed, 0f);
                }
                else
                {
                    theRB.velocity = new Vector2(0f, vertical * moveSpeed);
                }
            }
            else if (isMovingHorizontal)
            {
                theRB.velocity = new Vector2(horizontal * moveSpeed, 0f);
                wasMovingVertical = false;
            }
            else if (isMovingVertical)
            {
                theRB.velocity = new Vector2(0f, vertical * moveSpeed);
                wasMovingVertical = true;
            }
            else
            {
                theRB.velocity = Vector2.zero;
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
    }

    private void ClampPlayer()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    private void AnimatePlayer()
    {
        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }
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
