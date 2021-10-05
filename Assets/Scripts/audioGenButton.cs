using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;


public class audioGenButton : MonoBehaviour
{
    public Button SynthesizeButton;
    public string SpeechServiceSubscriptionKey = "";
    public string SpeechServiceRegion = "";
    public bool IsWaitingForSynthesis = false;
    public string GeneratedFileName = "azureSpeechSynth.wav";
    public AudioSource audioSource;

    private SpeechConfig config;
    private AudioConfig audioConfig;
    private string outputPath;
    private string buttonText = "";
    private AudioClip _audioClip;

    public async void OnButtonPressed()
    {
        Debug.Log("Button clicked");
        if (SynthesizeButton != null)
        {
            SynthesizeButton.interactable = false;
            setButtonText("...");
        }

        if (config is null)
        {
            outputPath = Path.Combine(Application.dataPath, GeneratedFileName);
            config = SpeechConfig.FromSubscription(SpeechServiceSubscriptionKey, SpeechServiceRegion);
            config.SpeechSynthesisLanguage = "en-US";
            config.SpeechSynthesisVoiceName = "en-ZA-LukeNeural"; //"en-US-JennyMultilingualNeural";
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff24Khz16BitMonoPcm);
            audioConfig = AudioConfig.FromWavFileOutput(outputPath);
        }
       
        Debug.Log("will save the file to: " + outputPath);


        var result = await GenerateTextAsync("We're all puppets   April!   I'm just a puppet who can see the strings.");
        switch (result.Reason)
        {
            case ResultReason.NoMatch:
                buttonText = "No Match";
                break;
            case ResultReason.Canceled:
                buttonText = "Canceled";
                break;
            case ResultReason.SynthesizingAudio:
                buttonText = "Synthesizing Audio";
                break;
            case ResultReason.SynthesizingAudioCompleted:
                buttonText = "Synthesizing Audio Completed";
                break;
            case ResultReason.SynthesizingAudioStarted:
                buttonText = "Synthesizing Audio Started";
                break;
            default:
                return;
        }

        Debug.Log(buttonText);
        setButtonText(buttonText);

        if (result.Reason != ResultReason.SynthesizingAudioCompleted)
        {
            SynthesizeButton.interactable = false;
        }

        _audioClip = await GetAudioClip(outputPath, AudioType.WAV);
        audioSource.Stop();
        audioSource.PlayOneShot(_audioClip, 1.0f);
        SynthesizeButton.interactable = true;
        StartCoroutine(ResetButtonTextAfter(4.0f, "Regenerate Audio", SynthesizeButton));
    }

    private void setButtonText(string text)
    {
        if (SynthesizeButton is null)
        {
            Debug.Log("Button not found, can't set its text");
            return;
        }
        var textField = SynthesizeButton.GetComponentInChildren<Text>();
        if (textField is null)
        {
            Debug.Log("Text not found");
        }
        else
        {
            textField.text = text;
        }
    }

    private async Task<SpeechSynthesisResult> GenerateTextAsync(string text)
    {
        using var synthesizer = new SpeechSynthesizer(config, audioConfig);
        SpeechSynthesisResult speechSynthesisResult = await synthesizer.SpeakTextAsync(text);
        return speechSynthesisResult;
    }

    public async Task<AudioClip> GetAudioClip(string filePath, AudioType fileType)
    {

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, fileType))
        {
            var result = www.SendWebRequest();

            while (!result.isDone) { await Task.Delay(100); }

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                return DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }


    IEnumerator ResetButtonTextAfter(float delayTime, string text, Button button)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        var textField = button.GetComponentInChildren<Text>();
        if (textField is null)
        {
            Debug.Log("Text not found");
        }
        else
        {
            textField.text = text;
            Debug.Log("Button text updated");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // register the click handler
        SynthesizeButton.onClick.AddListener(OnButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
