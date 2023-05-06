// ConversationManager.cs

using System.IO;
using UnityEngine;
using YagizAyer.Root.Scripts.ElevenLabsApiBase;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.Managers
{
    public class ConversationManager : SingletonBase<ConversationManager>
    {
        [SerializeField]
        [Header("ElevenLabs Settings")]
        private ElevenLabsApiClient elevenLabsAc;

        [SerializeField]
        [Header("OpenAI Settings")]
        private CompletionPreset completionSettings;

        [SerializeField]
        private AudioPreset audioSettings;

        [SerializeField]
        [Header("References")]
        private AudioSource npcAudioSource;

        [SerializeField]
        private TextAsset chatInstructions;

        [SerializeField]
        private TextAsset chatHistory;

        // player audio file -> transcription
        internal static void RequestPlayerAudioTranscription(string path) =>
            OpenAIApiClient.RequestFormAsync(path, Instance.audioSettings,
                onComplete: json =>
                {
                    var audioTranscription = AudioResponseData.FromJson(json).Text;
                    Debug.Log("prompt : " + audioTranscription);
                    Instance.RequestTextScoring(audioTranscription);
                });

        // transcription -> Npc Answer
        private void RequestTextScoring(string prompt) =>
            OpenAIApiClient.RequestJsonAsync(chatInstructions.text + chatHistory.text + prompt,
                completionSettings,
                onComplete: response =>
                {
                    if (response == null) return;

                    var fullAnswer = CompletionResponseData.FromJson(response).Choices[0].Text;
                    var splitAnswer = fullAnswer.Split('|');
                    var score = splitAnswer[0].Trim();
                    var answer = splitAnswer[1].Trim();

                    // save prompt and answer to chat history
                    var chatHistoryFile = new StreamWriter(@"Assets/Resources/OpenAIApi/ChatHistory.txt", true);
                    chatHistoryFile.WriteLine(prompt);
                    chatHistoryFile.WriteLine("\nNpc: " + fullAnswer + "\nPlayer: ");
                    chatHistoryFile.Close();

                    elevenLabsAc.RequestAsync(answer, onComplete: clip =>
                    {
                        npcAudioSource.PlayOneShot(clip);
                    });

                    Channels.NpcAnswering.Raise((score + "/" + answer).ToPassableData());
                });
    }
}