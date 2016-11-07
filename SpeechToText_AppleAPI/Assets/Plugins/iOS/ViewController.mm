//
//  ViewController.m
//  SpeechToText
//

#import "ViewController.h"
#import <Speech/Speech.h>

@interface ViewController () {
       
    // Speech recognize
    SFSpeechRecognizer *speechRecognizer;
    SFSpeechAudioBufferRecognitionRequest *recognitionRequest;
    SFSpeechRecognitionTask *recognitionTask;
    SFSpeechURLRecognitionRequest *urlRequest;    
    // Record speech using audio Engine
    AVAudioInputNode *inputNode;
    AVAudioEngine *audioEngine;
    
}

@end

@implementation ViewController


- (void)InitSpeed{   
	
    audioEngine = [[AVAudioEngine alloc] init];
    
    NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:@"en-US"];
    speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
    
    
    for (NSLocale *locate in [SFSpeechRecognizer supportedLocales]) {
        NSLog(@"%@", [locate localizedStringForCountryCode:locate.countryCode]);
    }
    // Check Authorization Status
    // Make sure you add "Privacy - Microphone Usage Description" key and reason in Info.plist to request micro permison
    // And "NSSpeechRecognitionUsageDescription" key for requesting Speech recognize permison
    [SFSpeechRecognizer requestAuthorization:^(SFSpeechRecognizerAuthorizationStatus status) {
        
        /*
         The callback may not be called on the main thread. Add an
         operation to the main queue to update the record button's state.
         */
        dispatch_async(dispatch_get_main_queue(), ^{
            switch (status) {
                case SFSpeechRecognizerAuthorizationStatusAuthorized: {
					// Ok
                    break;
                }
                case SFSpeechRecognizerAuthorizationStatusDenied: {
					[self CallbackStatus: "User denied access to speech recognition"];
                    break;
                }
                case SFSpeechRecognizerAuthorizationStatusRestricted: {
					[self CallbackStatus: "User denied access to speech recognition"];
                    break;
                }
                case SFSpeechRecognizerAuthorizationStatusNotDetermined: {
					[self CallbackStatus: "User denied access to speech recognition"];
                    break;
                }
            }
        });
        
    }];
}

// recording
- (void)startRecording {
	if (!audioEngine.isRunning) {
		if (recognitionTask) {
			[recognitionTask cancel];
			recognitionTask = nil;
		}
		
		[self CallStart: "Start"];
		
		AVAudioSession *session = [AVAudioSession sharedInstance];
		[session setCategory:AVAudioSessionCategoryRecord mode:AVAudioSessionModeMeasurement options:AVAudioSessionCategoryOptionDefaultToSpeaker error:nil];
		[session setActive:TRUE withOptions:AVAudioSessionSetActiveOptionNotifyOthersOnDeactivation error:nil];
		
		inputNode = audioEngine.inputNode;

		recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
		recognitionRequest.shouldReportPartialResults = NO;    
		AVAudioFormat *format = [inputNode outputFormatForBus:0];
		
		[inputNode installTapOnBus:0 bufferSize:1024 format:format block:^(AVAudioPCMBuffer * _Nonnull buffer, AVAudioTime * _Nonnull when) {
			[recognitionRequest appendAudioPCMBuffer:buffer];
		}];
		[audioEngine prepare];
		NSError *error1;
		[audioEngine startAndReturnError:&error1];
		NSLog(@"errorAudioEngine.description: %@", error1.description);
	}
}

- (void)stopRecording {
    
	 if (audioEngine.isRunning) {
         
        [self CallStop: "Stop"];
		recognitionTask =[speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error) {
			if (result != nil) {
				NSString *transcriptText = result.bestTranscription.formattedString;
				[self CallbackSpeechToText: [transcriptText UTF8String]];
			}
			else {
				[audioEngine stop];;
				recognitionTask = nil;
				recognitionRequest = nil;
				[self CallbackSpeechToText: "Null"];

			}
        }];
         
        // make sure you release tap on bus else your app will crash the second time you record.
        [inputNode removeTapOnBus:0];
		[audioEngine stop];
        [recognitionRequest endAudio];
         
			
	}
}

- (void)CallbackSpeechToText:(const char *)message {
    UnitySendMessage("speechtotext", "CallbackSpeechToText", message);
}
- (void)CallbackStatus:(const char *)message {
    UnitySendMessage("speechtotext", "CallbackStatus", message);
}
- (void)CallStart :(const char *)message{
    UnitySendMessage("speechtotext", "CallStart", message);
}
- (void)CallStop :(const char *)message{
    UnitySendMessage("speechtotext", "CallStop", message);
}

@end
extern "C"{
    ViewController *vc = [[ViewController alloc] init];    
    void _TAG_startRecording(){
        [vc startRecording];
    }
	void _TAG_InitSpeed(){
        [vc InitSpeed];
    }
	void _TAG_stopRecording(){
        [vc stopRecording];
    }    
}
