# Speech And Text in Unity iOS and Unity Android
Speech to text and text to speech in Unity iOS and Unity Android

I have provided all Java and Object-C source. You can learn how it works and add optimizations or new features.

## Native Speech and Text
* SpeechToText Android: https://developer.android.com/reference/android/speech/package-summary.html
* TextToSpeech Android: https://developer.android.com/reference/android/speech/tts/TextToSpeech.html
* SpeechToText iOS: https://developer.apple.com/reference/speech
* TextToSpeech iOS: https://developer.apple.com/reference/avfoundation

## Android
* To hide the default Android popup, there's a bool to enable and disable it, if you don't want the popup to show up.
```csharp
class SpeechToText
{
      public bool isShowPopupAndroid = false;
      ...
```
* Merge file AndroidManifest (If you want skip the default popup)

## Tutorial Config in Xcode
* Requires Xcode8 or higher. Target iOS 10.0.
* Add frameworks

      - speech.farmwork
      - AVFoundation.framework
      
* Add permissions

      - Privacy – Microphone Usage Description      
      - Privacy – Speech Recognition Usage Description
