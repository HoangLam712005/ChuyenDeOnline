using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System.Collections.Generic;

public class ChatManager : NetworkBehaviour
{
    [Header("UI Components")]
    public GameObject chatPanel;
    public Button chatButton;
    public InputField inputField;
    public Button sendButton;
    public Text chatText;

    private List<string> messages = new List<string>();

    private void Start()
    {
        if (chatPanel == null || chatButton == null || inputField == null || sendButton == null || chatText == null)
        {
            Debug.LogError("ChatManager: Một hoặc nhiều UI Elements chưa được gán trong Inspector!");
            return;
        }

        chatPanel.SetActive(false);
        chatButton.onClick.AddListener(ToggleChatPanel);
        sendButton.onClick.AddListener(SendMessage);
    }

    private void ToggleChatPanel()
    {
        chatPanel.SetActive(!chatPanel.activeSelf);
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            string message = inputField.text;
            inputField.text = "";

            string senderName = PlayerNicknameManager.GetNickname(Runner.LocalPlayer); // Lấy tên người gửi
            SendChatMessageRpc(senderName, message); // Gửi RPC
        }
    }

    // ✅ CHỈ GIỮ MỘT HÀM RPC NÀY THÔI!
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void SendChatMessageRpc(string senderName, string message)
    {
        string formattedMessage = $"{senderName}: {message}";
        messages.Add(formattedMessage);
        UpdateChatDisplay();
    }

    private void UpdateChatDisplay()
    {
        chatText.text = "";

        foreach (var msg in messages)
        {
            chatText.text += msg + "\n";
        }
    }
}
