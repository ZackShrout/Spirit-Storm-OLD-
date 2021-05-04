using SpiritStorm.Dialogue;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpiritStorm.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] Text text;
        [SerializeField] Text speaker;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject dialogue;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] float typeSpeed = .02f;
        bool isTyping = false;
        bool cancelTyping = false;
        bool firstButtonSelected = false;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            
            UpdateUI();
        }

        void Update()
        {
            if (Input.GetButtonUp("Submit") && playerConversant.HasNext())
            {
                if (!isTyping && !playerConversant.IsChoosing())
                {
                    playerConversant.Next();
                }
                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }
            else if (Input.GetButtonUp("Submit") && !playerConversant.HasNext() && !isTyping)
            {
                firstButtonSelected = false;
                playerConversant.Quit();
            }
        }

        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive()) return;
            speaker.text = playerConversant.GetSpeaker();
            dialogue.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                StartCoroutine(TextScroll(playerConversant.GetText()));
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform child in choiceRoot.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<Text>();
                textComp.text = choice.GetText();
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choice);
                });

                if (!firstButtonSelected)
                {
                    SetFirstButtonSelection(choiceInstance);
                }
            }
        }

        private void SetFirstButtonSelection(GameObject choiceInstance)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(choiceInstance);

            firstButtonSelected = true;
        }

        private IEnumerator TextScroll(string lineOfText)
        {
            int letter = 0;
            text.text = "";
            isTyping = true;
            cancelTyping = false;
            while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
            {
                text.text += lineOfText[letter];
                letter += 1;
                yield return new WaitForSeconds(typeSpeed);
            }
            text.text = lineOfText;
            isTyping = false;
            cancelTyping = false;
        }
    }

}