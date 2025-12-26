using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        public string name;
        public GameObject characterObject;
        public string[] dialogues;
        public bool isInteractable = true;
    }

    [Header("Character Settings")]
    public Character[] characters;
    public float interactionDistance = 3f;

    [Header("UI Settings")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text characterNameText;
    public Button nextButton;

    private int currentCharacterIndex = -1;
    private int currentDialogueIndex = 0;
    private bool isInDialogue = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextDialogue);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInDialogue)
        {
            TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isInDialogue)
        {
            EndDialogue();
        }
    }

    void TryInteract()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].characterObject != null && characters[i].isInteractable)
            {
                float distance = Vector3.Distance(transform.position, characters[i].characterObject.transform.position);
                if (distance <= interactionDistance)
                {
                    StartDialogue(i);
                    break;
                }
            }
        }
    }

    void StartDialogue(int characterIndex)
    {
        currentCharacterIndex = characterIndex;
        currentDialogueIndex = 0;
        isInDialogue = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // Play interaction sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayInteractionSound();
        }

        ShowCurrentDialogue();
    }

    void ShowCurrentDialogue()
    {
        if (currentCharacterIndex >= 0 && currentCharacterIndex < characters.Length)
        {
            Character currentChar = characters[currentCharacterIndex];

            if (characterNameText != null)
                characterNameText.text = currentChar.name;

            if (currentDialogueIndex < currentChar.dialogues.Length)
            {
                if (dialogueText != null)
                    dialogueText.text = currentChar.dialogues[currentDialogueIndex];

                // Play character voice
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayCharacterVoice(currentCharacterIndex, currentDialogueIndex);
                }
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void NextDialogue()
    {
        currentDialogueIndex++;
        ShowCurrentDialogue();
    }

    void EndDialogue()
    {
        isInDialogue = false;
        currentCharacterIndex = -1;
        currentDialogueIndex = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}