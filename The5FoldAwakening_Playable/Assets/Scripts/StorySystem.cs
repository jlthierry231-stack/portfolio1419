using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StorySystem : MonoBehaviour
{
    [System.Serializable]
    public class StorySegment
    {
        public string title;
        public string content;
        public string[] objectives;
        public bool isCompleted = false;
    }

    [Header("Story Settings")]
    public StorySegment[] storySegments;
    public int currentStoryIndex = 0;

    [Header("UI Settings")]
    public GameObject storyPanel;
    public Text storyTitleText;
    public Text storyContentText;
    public GameObject objectivesPanel;
    public Text objectivesText;

    [Header("Game Settings")]
    public GameObject playerObject;
    public CharacterManager characterManager;

    private bool storyInitialized = false;

    void Start()
    {
        InitializeStory();
    }

    void InitializeStory()
    {
        if (!storyInitialized && storySegments.Length > 0)
        {
            ShowStory(currentStoryIndex);
            storyInitialized = true;
        }
    }

    public void ShowStory(int index)
    {
        if (index >= 0 && index < storySegments.Length)
        {
            StorySegment segment = storySegments[index];

            if (storyTitleText != null)
                storyTitleText.text = segment.title;

            if (storyContentText != null)
                storyContentText.text = segment.content;

            UpdateObjectivesDisplay();

            if (storyPanel != null)
                storyPanel.SetActive(true);

            // Play narrator voice for story segment
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayNarratorVoice(index);
            }
        }
    }

    void UpdateObjectivesDisplay()
    {
        if (objectivesText != null && storySegments.Length > currentStoryIndex)
        {
            StorySegment currentSegment = storySegments[currentStoryIndex];
            string objectivesStr = "Objectives:\n";

            for (int i = 0; i < currentSegment.objectives.Length; i++)
            {
                string status = currentSegment.isCompleted ? "[✓]" : "[ ]";
                objectivesStr += status + " " + currentSegment.objectives[i] + "\n";
            }

            objectivesText.text = objectivesStr;
        }
    }

    public void CompleteObjective(int objectiveIndex)
    {
        if (currentStoryIndex < storySegments.Length)
        {
            // Mark objective as completed (simplified - in a real game you'd track individual objectives)
            storySegments[currentStoryIndex].isCompleted = true;
            UpdateObjectivesDisplay();

            // Play objective complete sound
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayObjectiveCompleteSound();
            }

            // Check if ready to advance story
            CheckStoryProgression();
        }
    }

    void CheckStoryProgression()
    {
        // Simplified progression - advance when current segment is completed
        if (storySegments[currentStoryIndex].isCompleted && currentStoryIndex < storySegments.Length - 1)
        {
            currentStoryIndex++;

            // Play story advance sound
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayStoryAdvanceSound();
            }

            ShowStory(currentStoryIndex);
        }
    }

    public void HideStory()
    {
        if (storyPanel != null)
            storyPanel.SetActive(false);
    }

    void Update()
    {
        // Handle story panel input
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (storyPanel != null && storyPanel.activeSelf)
            {
                HideStory();
            }
            else
            {
                ShowStory(currentStoryIndex);
            }
        }
    }
}