using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiritStorm.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] PlayerConversant playerConversant = null;
        bool canActivate = false;
        bool isTalking = false;

        private void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationEnded += EndTalking;
        }

        private void Update()
        {
            if (canActivate && !isTalking && Input.GetButtonUp("Submit"))
            {
                if (dialogue == null) return;

                playerConversant.StartDialogue(this, dialogue);
                StartTalking();
            }
        }

        private void StartTalking()
        {
            isTalking = true;
        }

        private void EndTalking()
        {
            isTalking = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                canActivate = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                canActivate = false;
            }
        }
    }
}
