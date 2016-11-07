# SpeechToText_UnityIOS_AppleAPI
Speed to text in Unity iOS use Native Speech Recognition

## Use Native Speech Recognition
* At WWDC 2016, iOS 10 introduced a new API that supports continuous speech recognition and helps you develop apps that can recognize and transcribe speech it into text.
* Supporting 58 popular languages, it is easy to implement and provides very accurate results (in my opinion). It is now time to forget about third-party frameworks.
* In this article we will show you how to use Speech framework in your application and call native from Unity

  Note: Requires Xcode8 or higher. Target iOS 10.0
  
  Detail speech API: /reference/speech

## Tutorial
More detail: https://pingyolo.wordpress.com/2016/07/26/speech-to-text-in-unity-ios/

* Step 1: Build Sample project

  switch Unity iOS platform -> build iOS -> You will have Xcode project.

* Step 2: Open xcode project with Xcode 8 or higher

* Step 3: You need to add speech.farmwork to your project.

* Step 4:  
  Add “Privacy – Microphone Usage Description” key and reason in Info.plist to request micro permison

  Add “Privacy – Speech Recognition Usage Description” key and reason in Info.plist to request Speech recognize permison

* Step 5: Build and run in iOS Device
