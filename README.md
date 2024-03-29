# UnityAzureSpeechSynthesis

Quick demo showing how to use Azure Speech Synthesis (tts) from Unity and loading/playing the generated file in game.

The script/logic is in `Assets\Scripts\AudioGenButton.cs` which is used by the Button Manager (inside `AudioGenUI > Plane > Canvas`)
The Azure Speech Service API key must be set the UI for the code to work.

## Required Unity package

The Unity Speech SDK must be installed manually in your project and is available at https://aka.ms/csspeech/unitypackage
The SDK was too big for GitHub and you need to install it manually, Window > Package Manager > + icon and select `Add package from Disk` Then pick the zip file you just downloaded with the latest speech package.

## Azure setup

You can setup a free sandbox and create an Azure Speech Resource to get your API key, follow these instructions to get setup:

https://docs.microsoft.com/en-us/learn/modules/create-language-translator-mixed-reality-application-unity-azure-cognitive-services/3-exercise-create-azure-speech-resource

## Available languages and options

The tts settings can be configured by modifying the config opjects in the script. Here is a list of supported languages and voices:
https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support

TTS concepts and options are explained here: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/index-text-to-speech 

## Demo setup (Azure key)

Make sure to set your Azure Speech Resource Key in the Script attributes.

![Screenshot 2021-10-05 124007](https://user-images.githubusercontent.com/113/136091702-ff381102-e73d-43ee-9fb6-c0d4425b2e34.png)

To access the inspector panel, you have to click on the button manager which you can find in the Hierarchy pane shown to your left.
The inspector shows the AudioGenButton script with its public members. The Subscriber key and region can be set there.

Note that an Audio Source is passed to the script and the provided audio source uses a mixer you can customize (uneeded for this demo).


## How the code works

The logic is pretty straightforward

1. Our script adds a click event handler for the UI button (in the `Start()` function)
2. when the button is pressed, `OnButtonPressed()` is called, which sets up the configuration and caches it (this snippet should really be moved to `Start()`)
3. The configuration sets the path we want the Azure tts Speech API to use when generating the audio file
4. We trigger the text generation and wait for it to be done (an audio file is created in the user's asset folder)
5. if everything went well, the file is loaded in an audio clip (via the custom async `GetAudioClip` function)
6. the audiosource is stopped and the new clip is played at full volume.
7. We also start a coroutine (code executed in parallel) to reset the button text after 4s)
