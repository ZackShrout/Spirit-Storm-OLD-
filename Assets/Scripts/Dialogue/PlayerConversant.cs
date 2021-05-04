using SpiritStorm.Control;
using SpiritStorm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpiritStorm.Dialogue
{
    public class PlayerConversant : MonoBehaviour, IAction
    {
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;
        string nameOfChooser = null;

        public event Action onConversationUpdated;
        public event Action onConversationEnded;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
            GetComponent<PlayerController>().enabled = false;
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
            onConversationEnded();
            GetComponent<PlayerController>().enabled = true;
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }
        
        public bool IsChoosing()
        {
            return isChoosing;
        }
        
        public string GetText()
        {
            if (currentNode == null) return "";

            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public string GetSpeaker()
        {
            if (currentNode == null) return "";

            if (isChoosing) return nameOfChooser;
            
            return currentNode.GetSpeaker();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 1)
            {
                isChoosing = true;
                TriggerExitAction();
                nameOfChooser = currentDialogue.GetPlayerChildren(currentNode).First<DialogueNode>().GetSpeaker();
                onConversationUpdated();
                return;
            }

            DialogueNode[] childNodes = currentDialogue.GetAllChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, childNodes.Count());
            TriggerExitAction();
            currentNode = childNodes[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        private void TriggerEnterAction()
        {
            TriggerAction(currentNode.GetOnEnterAction());    
        }

        private void TriggerExitAction()
        {
            TriggerAction(currentNode.GetOnExitAction());
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>()) 
            {
                trigger.Trigger(action);
            }
        }

        public void CancelAction()
        {
            
        }
    }
}
