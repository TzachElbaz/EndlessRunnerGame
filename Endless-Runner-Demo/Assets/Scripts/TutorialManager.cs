using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial State")]
    public int stage = 1;

    [Header("UI Instruction Elements")]
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject roll;
    [SerializeField] private GameObject doubleJump;
    [SerializeField] private GameObject jumpAndRoll;
    [SerializeField] private GameObject rollAndJump;
    [SerializeField] private GameObject platformJump; // Added platformJump UI element
    [SerializeField] private GameObject startGame;
    [SerializeField] private Platform platform;

    [Header("Timing Settings")]
    [Tooltip("The time window in seconds to complete a combo action.")]
    [SerializeField] private float comboTimeWindow = 0.6f;

    [Tooltip("Delay in seconds before moving to the next instruction.")]
    [SerializeField] private float transitionDelay = 0.5f;

    private float lastUpTime = -10f;
    private float lastDownTime = -10f;

    // This prevents the player from inputting commands during the delay
    private bool isTransitioning = false;

    // Changed to public so your player collision script can change it to true when they land
    public bool isJumpOnPlatform = false;

    void Start()
    {
        UpdateTutorialUI();
    }

    void Update()
    {
        // If we are currently waiting for the next stage to load, ignore all inputs
        if (isTransitioning) return;

        switch (stage)
        {
            case 1: // Jump
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    AdvanceStage();
                }
                break;

            case 2: // Roll
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    AdvanceStage();
                }
                break;

            case 3: // DoubleJump
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    if (Time.time - lastUpTime <= comboTimeWindow)
                    {
                        AdvanceStage();
                    }
                    else
                    {
                        lastUpTime = Time.time;
                    }
                }
                break;

            case 4: // Jump-Roll
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    lastUpTime = Time.time;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    if (Time.time - lastUpTime <= comboTimeWindow)
                    {
                        AdvanceStage();
                    }
                }
                break;

            case 5: // Roll-Jump
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    lastDownTime = Time.time;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    if (Time.time - lastDownTime <= 0.6f)
                    {
                        AdvanceStage();
                    }
                }
                break;

            case 6: // Platform Jump (New Stage)
                platform.isMooving = true;
                if (isJumpOnPlatform)
                {
                    // Reset to false to prevent it from triggering immediately if used again later
                    isJumpOnPlatform = false;
                    AdvanceStage();
                }
                break;
        }
    }

    // Instead of advancing instantly, we start the Coroutine delay
    private void AdvanceStage()
    {
        StartCoroutine(TransitionToNextStage());
    }

    // The Coroutine that handles the delay
    private IEnumerator TransitionToNextStage()
    {
        // 1. Lock inputs so the player can't spam buttons
        isTransitioning = true;

        // 2. Turn off all UI elements to create a visual "blank" gap for 0.5s
        HideAllElements();

        // 3. Wait for the specified delay time
        yield return new WaitForSeconds(transitionDelay);

        // 4. Increase stage, update UI to show the new element, and unlock inputs
        stage++;
        UpdateTutorialUI();
        isTransitioning = false;
    }

    // Helper method to hide everything during the transition delay
    private void HideAllElements()
    {
        jump.SetActive(false);
        roll.SetActive(false);
        doubleJump.SetActive(false);
        jumpAndRoll.SetActive(false);
        rollAndJump.SetActive(false);
        platformJump.SetActive(false); // Hide the new UI element
    }

    private void UpdateTutorialUI()
    {
        jump.SetActive(stage == 1);
        roll.SetActive(stage == 2);
        doubleJump.SetActive(stage == 3);
        jumpAndRoll.SetActive(stage == 4);
        rollAndJump.SetActive(stage == 5);
        platformJump.SetActive(stage == 6); // Show the new UI element on stage 6

        startGame.SetActive(stage > 6); // Push startGame to stage 7 and beyond
    }

    public void OnStartGameClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in the Build Settings!");
        }
    }
}