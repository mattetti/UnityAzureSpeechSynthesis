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

https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support

## Demo setup (Azure key)

Make sure to set your Azure Speech Resource Key in the Script attributes.
