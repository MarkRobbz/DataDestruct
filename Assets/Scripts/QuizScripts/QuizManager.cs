using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionTextPromptUI;
    public TextMeshProUGUI answersPromptTextUI;
    public List<Button> answerButtons;
    public List<QuizData> quizDatasets;
    public Button continueButton;
    private List<Question> remainingQuestions;
    private Question currentQuestion;
    private int currentQuizIndex = -1;
    private QuizTrigger currentQuizTrigger;
    private Color defaultTextColor;
    private float defaultFontSize;
    private AudioManager audioManager;
    

    private void Start()
    {
        defaultTextColor = answersPromptTextUI.color;
        defaultFontSize = answersPromptTextUI.fontSize;
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    public void InitializeQuiz(int quizIndex, QuizTrigger quizTrigger)
    {
        if (quizIndex >= 0 && quizIndex < quizDatasets.Count)
        {
            currentQuizIndex = quizIndex;
            remainingQuestions = new List<Question>(quizDatasets[quizIndex].questions);
            LoadNextQuestion();
            currentQuizTrigger = quizTrigger;
        }
        else
        {
            Debug.LogError("Quiz index out of range: " + quizIndex);
        }
    }

    void LoadNextQuestion()
    {
        if (currentQuizIndex == -1)
        {
            Debug.LogError("No quiz initialised!");
            return;
        }

        if (remainingQuestions.Count > 0)
        {
            int index = Random.Range(0, remainingQuestions.Count);
            currentQuestion = remainingQuestions[index];
            remainingQuestions.RemoveAt(index);

            questionTextPromptUI.text = currentQuestion.questionText;

            // Hide the continue button when loading a new question
            continueButton.gameObject.SetActive(false);

            // Reset previous feedback for new question
            answersPromptTextUI.enableAutoSizing = false;
            answersPromptTextUI.color = defaultTextColor;
            answersPromptTextUI.fontSize = defaultFontSize;
            answersPromptTextUI.alignment = TextAlignmentOptions.TopLeft;
            answersPromptTextUI.text = "";

            SetAnswerOptionsAndListeners();
        }
        else
        {
            Debug.Log("Quiz completed!");
            CompleteQuiz();
        }
    }

    private void SetAnswerOptionsAndListeners()
    {
        string allAnswersText = "";
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            allAnswersText += $"{(char)('A' + i)}. {currentQuestion.answers[i]}\n\n"; //adds newline for space
            SetButtonActiveAndListener(answerButtons[i], i < currentQuestion.answers.Length, i);
        }
        answersPromptTextUI.alignment = TextAlignmentOptions.Left;
        answersPromptTextUI.text = allAnswersText.TrimEnd(new char[] { '\n' }); // Trim last newline for aesthtics
    }


    private void SetButtonActiveAndListener(Button button, bool isActive, int answerIndex)
    {
        button.gameObject.SetActive(isActive);
        button.interactable = isActive;
        if (isActive)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => AnswerQuestion(answerIndex));
        }
    }

    public void AnswerQuestion(int index)
    {
        bool isCorrect = index == currentQuestion.correctAnswerIndex;
        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }

        // Hide  continue button for next question
        continueButton.gameObject.SetActive(false);

        // handle feedback and detailed answers
        StartCoroutine(DisplayFeedbackAndDetailedAnswers(isCorrect));
    }

    private IEnumerator DisplayFeedbackAndDetailedAnswers(bool isCorrect)
    {
        // Display feedback (auto sizes)
        answersPromptTextUI.enableAutoSizing = true;
        answersPromptTextUI.fontSizeMin = 10;
        answersPromptTextUI.fontSizeMax = 100;
        answersPromptTextUI.alignment = TextAlignmentOptions.Center;
        answersPromptTextUI.text = isCorrect ? "Correct" : "Incorrect";
        answersPromptTextUI.color = isCorrect ? Color.green : Color.red;

        yield return new WaitForSecondsRealtime(2f);
        
        answersPromptTextUI.color = defaultTextColor; //reset text colour for answers
        answersPromptTextUI.enableAutoSizing = false;
        answersPromptTextUI.fontSize = defaultFontSize;

        DisplayDetailedAnswers();

        // Make continue button active after feedback
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(LoadNextQuestion);
        continueButton.gameObject.SetActive(true);
    }

    private void DisplayDetailedAnswers()
    {
        string infoText = "";
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            // prefix and colour answer text
            if (i == currentQuestion.correctAnswerIndex)
            {
                infoText += $"<color=green>{(char)('A' + i)}. {currentQuestion.answers[i]}</color>\n";
            }
            else
            {
                infoText += $"<color=red>{(char)('A' + i)}. {currentQuestion.answers[i]}</color>\n";
            }

            // dont colour the extra info
            infoText += $"{currentQuestion.answerInfo[i]}\n\n";
        }
        answersPromptTextUI.text = infoText;
        answersPromptTextUI.alignment = TextAlignmentOptions.Left;

        ScrollToTop();
    }


    private void ScrollToTop()
    {
        ScrollRect scrollRect = answersPromptTextUI.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            StartCoroutine(DelayedScrollToTop(scrollRect));
        }
    }

    private IEnumerator DelayedScrollToTop(ScrollRect scrollRect)
    {
        // Wait for end of frame (ensures UI upated)
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 1f; // Scrolls to the top
    }

    public void CompleteQuiz()
    {
        if (currentQuizIndex == -1)
        {
            Debug.LogError("No quiz initialised!");
            return;
        }

        questionTextPromptUI.text = "Quiz completed!";
        answersPromptTextUI.text = "";

        
        if (audioManager != null)
        {
            AudioClip clipToPlay = audioManager.GetSoundEffect(3); 
            if (clipToPlay != null)
            {
                audioManager.PlaySFX(clipToPlay);
            }
            else
            {
                Debug.LogError("Sound effect at index 3 is not set.");
            }
        }
        else
        {
            Debug.LogError("AudioManager is not properly configured.");
        }

        GameManager.instance.HideQuiz();
        if (currentQuizTrigger != null)
        {
            currentQuizTrigger.RevealKey();
            Destroy(currentQuizTrigger.gameObject); // Make sure to destroy the GameObject, not the script
        }
        else
        {
            Debug.LogError("No QuizTrigger was set for this quiz.");
        }
    }

}
