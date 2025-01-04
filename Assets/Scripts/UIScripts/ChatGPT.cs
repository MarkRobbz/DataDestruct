using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatGPT : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text textChat;
    public TMP_InputField inputField;
    public Button sendBtn;
    public Button closeButton;


    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    private const string API_KEY = "Removed"; // Replace with a GPT API Key
    private const string MODEL_NAME = "gpt-3.5-turbo"; // GPT model name (ChatGPT 3.5 is the current API I can get for low price)

    [System.Serializable]
    public class OpenAIResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    private void Start()
    {
        sendBtn.onClick.AddListener(() => SendMessageToAPI(inputField.text));
        closeButton.onClick.AddListener(CloseChatUI);
        inputField.onEndEdit.AddListener(HandleEnterKey); //Can press enter key to send message too
    }

    public void SendMessageToAPI(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            // Define the instruction prefix to be added before the user's message
            string instructionPrefix = "Your name Is B.M.O (Binary Machine Online), you're here to assist a student to learn data strcutres and help explain them at a high schooler level please respond to this question with enthusiasm and keep them from veering off topic: ";

            // Concatenate the instruction with the user's message
            string prefixedMessage = instructionPrefix + message;

            // Clears input field and set placeholder text while waiting for response
            inputField.text = "";
            TMP_Text placeholder = inputField.placeholder as TMP_Text;
            if (placeholder != null)
            {
                placeholder.text = "Message Sent, Awaiting response...";
            }
            inputField.ActivateInputField(); //puts focus back on input field

            // Use the prefixed message instead of the original message
            StartCoroutine(CallChatGPTAPI(prefixedMessage, (response) =>
            {
                // Once response received clear placeholder text and display the response
                if (placeholder != null)
                {
                    placeholder.text = "";
                }

                textChat.text += "\nUser: " + message + "\n\n"; // Display original user message
                textChat.text += "B.M.O: " + response + "\n\n";


                Canvas.ForceUpdateCanvases();
                ScrollRect scrollRect = textChat.GetComponentInParent<ScrollRect>();
                if (scrollRect != null)
                {
                    scrollRect.verticalNormalizedPosition = 0f; // This scrolls to the bottom
                }

                inputField.ActivateInputField();
            }));
        }
    }

    IEnumerator CallChatGPTAPI(string message, System.Action<string> callback)
    {
        // Constructs JSON request body with the provided message and a model name.
        string requestBody = "{\"model\":\"" + MODEL_NAME + "\", \"messages\":[{\"role\":\"user\",\"content\":\"" + message + "\"}]}";
        Debug.Log("Sending request with body: " + requestBody); 

        var request = new UnityWebRequest(API_URL, "POST");

        // Converts body into a byte array 
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

        // handles response 
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);

        // Starts request and waits for it to complete.
        yield return request.SendWebRequest();

        // Checksresult of the request for errors.
        if (request.result != UnityWebRequest.Result.Success)
        {
          
            Debug.LogError(request.error);
            callback("Error contacting API");
        }
        else
        {
            // Extracts text response from the request
            string response = request.downloadHandler.text;
            OpenAIResponse jsonResponse = JsonUtility.FromJson<OpenAIResponse>(response);
            string chatGPTMessage = jsonResponse.choices[0].message.content;
            callback(chatGPTMessage);
        }
    }


    private void HandleEnterKey(string text)
    {
        if (!string.IsNullOrWhiteSpace(text) && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessageToAPI(text);
        }
    }

    public void OpenGPTUI()
    {
        // Show ChatGPT UI and pause game
        GameManager.instance.ToggleChatUI(true);
    }

    public void CloseChatUI()
    {
        GameManager.instance.ToggleChatUI(false);
    }
}
