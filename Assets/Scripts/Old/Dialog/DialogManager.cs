using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText, nameText;
    public GameObject dialogBox, nameBox;

    public string[] dialogLines;

    public int currentLine;

    public static DialogManager instance;

    private bool justStarted;

    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Submit"))
            {
                if (!justStarted)
                {
                    if (!isTyping)
                    {
                        currentLine++;

                        if (currentLine >= dialogLines.Length)
                        {
                            dialogBox.SetActive(false);

                            GameManager.instance.dialogActive = false;

                            if (shouldMarkQuest)
                            {
                                shouldMarkQuest = false;
                                if (markQuestComplete)
                                {
                                    QuestManager.instance.MarkQuestComplete(questToMark);
                                }
                                else
                                {
                                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                                }
                            }
                        }
                        else
                        {
                            CheckIfName();

                            StartCoroutine(TextScroll(dialogLines[currentLine]));
                        }
                    }
                    else if(isTyping && !cancelTyping)
                    {
                        cancelTyping = true;
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }

    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        dialogText.text = "";
        isTyping = true;
        cancelTyping = false;
        while(isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            dialogText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        dialogText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;

        currentLine = 0;

        CheckIfName();

        StartCoroutine(TextScroll(dialogLines[currentLine]));

        //dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;

        nameBox.SetActive(isPerson);

        GameManager.instance.dialogActive = true;
    }

    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
