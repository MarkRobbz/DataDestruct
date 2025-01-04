using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isGamePaused = false;

    //NPC Vars
    private NPCSystem currentNPC;
    public GameObject NPCDialogueUI;
    public GameObject chatUI;
    private bool isPlayerInteractingWithNPC = false;
    public bool IsChatGPTActive { get; private set; }

    public GameObject quizUI;

    //Array Alleway Vars
    public GameObject arrayHud;
    public GameObject arrayHudUITrigger;
    public GameObject TrashItemsParent;
    [SerializeField] private GameObject statueArrowsParent;

    public List<MapBoard> mapBoards = new List<MapBoard>(); // List of all MapBoards
    private int totalNodes = 3;
    private NodeInteraction currentTargetNode;
    private int nodesActivated = 0;
    private HashSet<GameObject> revealedRoutes = new HashSet<GameObject>();
    public Material activeNodeMaterial;
    // public List<GameObject> doors;
    public GameObject doorTrigger;

    //Tree Top Traversal Vars
    public TreeTraversal treeTraversal;

    // List of all sections
    public List<Section> sections;

    public CameraController cameraController;

    public int gemCount = 0; // Track the number of gems collected
    public TextMeshProUGUI gemCountText;
    
    [SerializeField]
    private GameObject tutorialTriggerObject; 
    private void Awake()
    {
        // Singleton pattern*
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        //if (treeTraversal == null)
        //{
        //    Debug.LogError("TreeTraversal is not assigned in GameManager.");
        //}
        
    }
    
    private void Start()
    {
        

        
    }


    //============GAME FUNCTIONS MANAGEMENT===============
    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
        UnlockCursor();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isGamePaused = false;
        LockCursor();
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }

    public void CompleteTaskInSection(string sectionName)
    {

        if (cameraController == null)
        {
            Debug.LogError("CameraController is not assigned in GameManager.");
            return;
        }

        var section = sections.Find(s => s.sectionName == sectionName);
        if (section != null && !section.isTaskComplete)
        {
            section.isTaskComplete = true;

            RevealQuizButton(section.quizButton); //Reveal quiz button object
            cameraController.SwitchToCamera(section.cinematicCamera);
            cameraController.AnimateCamera(section.cinematicCamera, section.cameraAnimation);
            StartCoroutine(SwitchBackToMainCameraAfterDelay(section.cameraAnimation.animationDuration));
        }
    }

    private IEnumerator SwitchBackToMainCameraAfterDelay(float delay) //Waits for the animation to finnish
    {
        yield return new WaitForSeconds(delay);
        cameraController.SwitchBackToMainCamera();
    }



    public void RevealQuizButton(GameObject quizButton)
    {
        if (quizButton != null)
        {
            quizButton.SetActive(true);
        }
        else
        {
            Debug.LogError("Quiz button reference is null.");
        }
    }


    public void CollectGem()  //Icreaseds the Gem count
    {
        gemCount++;
        UpdateGemCountUI();
    }


    //========LINKED LIST LABYRINTH==========
    public void UpdateAllMaps(NodeInteraction nodeInteraction, int nodeIndex)
    {
        nodesActivated++;

        // Change the color of the node on all map boards
        foreach (var mapBoard in mapBoards)
        {
            mapBoard.ChangeMapNodeColour(nodeIndex, activeNodeMaterial);
        }

        // Check if all nodes have been activated
        if (nodesActivated >= totalNodes)
        {
            EnableDoorTrigger();
            Debug.Log("Door Trigger enabled");
            instance.CompleteTaskInSection("LinkedListLabyrinth");
            return;
        }

        // If it's not last node reveal a new path
        GameObject selectedRoute = ChooseRandomRoute(nodeInteraction.possibleRoutesToNextNode);
        if (selectedRoute != null)
        {
            // Add the route to the revealedRoutes to track its state
            revealedRoutes.Add(selectedRoute);

            // reveal this route on all map boards
            foreach (var mapBoard in mapBoards)
            {
                GameObject correspondingRoute = mapBoard.routes.FirstOrDefault(r => r.name == selectedRoute.name);
                if (correspondingRoute != null)
                {
                    correspondingRoute.SetActive(true);
                }
            }
        }
    }

    public void SetNextNode(NodeInteraction nextNode)
    {
        currentTargetNode = nextNode;
        foreach (var mapBoard in mapBoards)
        {
            mapBoard.HighlightPathToNode(currentTargetNode);
        }
    }

    private GameObject ChooseRandomRoute(GameObject[] possibleRoutes)
    {
        List<GameObject> unrevealedRoutes = new List<GameObject>();
        foreach (var route in possibleRoutes)
        {
            if (!revealedRoutes.Contains(route))
            {
                unrevealedRoutes.Add(route);
            }
        }

        if (unrevealedRoutes.Count == 0)
        {
            return null; // No unrevealed routes available
        }

        int randomIndex = Random.Range(0, unrevealedRoutes.Count);
        return unrevealedRoutes[randomIndex];
    }

    private void EnableDoorTrigger()
    {
        if (doorTrigger != null)
        {
            doorTrigger.SetActive(true); // Enable the trigger
        }
        else
        {
            Debug.LogError("Door Trigger not assigned in GameManager.");
        }
    }


    //=============STACK OVERFLOW======================
    public void ToggleStatueArrows(bool show)
    {
        // Toggle the parent object to show/hide all arrow children
        if (statueArrowsParent != null)
        {
            statueArrowsParent.SetActive(show);
        }
        else
        {
            Debug.LogError("StatueArrowsParent is not assigned in the GameManager.");
        }
    }

    //===========TREE TOP TRAVERSAl==================
    public bool CheckPlayerJump(TreeNode node)
    {
        // Check if player has jumped on correct node
        if (treeTraversal != null)
        {
            return treeTraversal.IsCorrectNode(node);
        }
        else
        {
            Debug.LogError("TreeTraversal reference is not set in GameManager.");
            return false;
        }
    }




    //============UI MANAGEMENT METHODS===============
    public void ShowNPCDialogue()
    {
        NPCDialogueUI.SetActive(true);
        isPlayerInteractingWithNPC = true;
        UnlockCursor();
    }

    public void HideNPCDialogue()
    {
        NPCDialogueUI.SetActive(false);
        isPlayerInteractingWithNPC = false;
        LockCursor();

    }

    public void ToggleChatUI(bool show)
    {
        chatUI.SetActive(show);
        IsChatGPTActive = show; // Sest the ChatGPT UI active flag

        if (show)
        {
            PauseGame();
            HideNPCDialogue();
            UnlockCursor();
        }
        else
        {
            if (!isPlayerInteractingWithNPC)
            {
                ResumeGame();
            }
        }
    }

    public void ShowQuiz()
    {
        quizUI.SetActive(true);
        PauseGame();
    }

    public void HideQuiz()
    {
        quizUI.SetActive(false);
        ResumeGame();
    }

    public void UpdateGemCountUI() //Updates the gem UI number
    {
        if (gemCountText != null)
        {
            gemCountText.text = gemCount.ToString();
        }
        else
        {
            Debug.LogError("GemCountText is not assigned in GameManager.");
        }
    }




    //============CURSOR MANAGEMENT METHODS===============
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }






    //============NPC METHODS===============

    public void SetCurrentNPC(NPCSystem npc)
    {
        currentNPC = npc;
    }

    public void ClearCurrentNPC()
    {
        currentNPC = null;
    }

    public NPCSystem GetCurrentNPC()
    {
        return currentNPC;
    }



    public void OnInteractionWithNPC()
    {
        Debug.Log("OnInteractionWithNPC called."); // Debug log to confirm the method is called

        if (currentNPC != null)
        {
            Debug.Log("Current NPC Identifier: " + currentNPC.npcIdentifier); // Log the identifier

            if (currentNPC.npcIdentifier == "BMO_ArrayAlleyway")
            {
                ToggleArrayHud(currentNPC.npcIdentifier, true);
                HideNPCDialogue();
            }
            else if (currentNPC.npcIdentifier == "BMO_StackOverflow")
            {
                Debug.Log("Toggling Statue Arrows."); // Confirm this block is entered
                ToggleStatueArrows(true);
                HideNPCDialogue();
            }
            else if (currentNPC.npcIdentifier == "BMO_LinkedList")
            {
                HideNPCDialogue();
            }
            else if (currentNPC.npcIdentifier == "BMO_TreeTop")
            {
                HideNPCDialogue();
            }
            else if (currentNPC.npcIdentifier == "BMO_Tutorial")
            {
                HideNPCDialogue();
            } else if (currentNPC.npcIdentifier == "BMO_Hub")
            {
                HideNPCDialogue();
            }
            else
            {
                HideNPCDialogue();
                Debug.LogError("The NPC identifier is not recognized: " + currentNPC.npcIdentifier);
            }
        }
        else
        {
            HideNPCDialogue();
            Debug.LogError("No current NPC is set in GameManager.");
        }
    }


    public void ToggleArrayHud(string npcId, bool show)
    {
        if (npcId == "BMO_ArrayAlleyway" && arrayHud != null)
        {
            arrayHud.SetActive(show);
            LockCursor();
            HideNPCDialogue();
            arrayHudUITrigger.SetActive(show);
            TrashItemsParent.SetActive(show);


        }

    }



}

[System.Serializable]
public class Section
{
    public string sectionName; // ID for the section
    public GameObject quizButton; // Reference to the quiz button of this secition
    public bool isTaskComplete; // Indicate task is completed
    public CinemachineVirtualCamera cinematicCamera;
    public AnimationParameters cameraAnimation;
}

[System.Serializable]
public class AnimationParameters
{
    public Vector3 targetPosition; // Target pos for moving animation
    public float targetFOV; // Target field of view (zoom)
    public float animationDuration; // animation duration
    public bool usePositionAnimation; // Flag to see which animation to use (zoom or potion move)
}