# Speech And Text in Unity iOS and Unity Android
Speed to text and text to speed in Unity iOS and Unity Android
I have provide all java and object c source. you can know how it work, optimization, or add any features

## Native Speech and Text
* SpeechToText Android: https://developer.android.com/reference/android/speech/package-summary.html
* TextToSpeed Android: https://developer.android.com/reference/android/speech/tts/TextToSpeech.html
* SpeechToText iOS: https://developer.apple.com/reference/speech
* TextToSpeech iOS: https://developer.apple.com/reference/avfoundation

## Android
* Hide default android popup, there's a bool to enable and disable if you don't want the popup to show up.
```
class SpeechToText
{
      public const bool isShowPopupAndroid = false;
      ...
```
* Merge file AndroidManifest (If you want skip the default popup)
## Tutorial Config in Xcode
* Requires Xcode8 or higher. Target iOS 10.0
* Add farmwork

      - speech.farmwork
      - AVFoundation.framework
      
* Add permission

      - Privacy – Microphone Usage Description      
      - Privacy – Speech Recognition Usage Description
