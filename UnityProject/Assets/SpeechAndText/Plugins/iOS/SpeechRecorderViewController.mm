//
//  SpeechRecorderViewController.m
//  SpeechToText
//
#import "SpeechRecorderViewController.h"
#import <Speech/Speech.h>

@interface SpeechRecorderViewController ()
{    
    // Speech recognize
    SFSpeechRecognizer *speechRecognizer;
    SFSpeechAudioBufferRecognitionRequest *recognitionRequest;
    SFSpeechRecognitionTask *recognitionTask;
    // Record speech using audio Engine
    AVAudioInputNode *inputNode;
    AVAudioEngine *audioEngine;	
	NSString * LanguageCode;
    
}
@end

@implementation SpeechRecorderViewController

- (id)init
{
	audioEngine = [[AVAudioEngine alloc] init];
    LanguageCode = @"ko-KR";
    NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
    speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
    
    //for (NSLocale *locate in [SFSpeechRecognizer supportedLocales]) {
    //    NSLog(@"%@", [locate localizedStringForCountryCode:locate.countryCode]);
    //}
	
    // Check Authorization Status
    // Make sure you add "Privacy - Microphone Usage Description" key and reason in Info.plist to request micro permison
    // And "NSSpeechRecognitionUsageDescription" key for requesting Speech recognize permison
    [SFSpeechRecognizer requestAuthorization:^(SFSpeechRecognizerAuthorizationStatus status) {
        //The callback may not be called on the main thread. Add an operation to the main queue to update the record button's state.
        dispatch_async(dispatch_get_main_queue(), ^{
            switch (status) {
                case SFSpeechRecognizerAuthorizationStatusAuthorized: {
                    NSLog(@"SUCCESS");
                    break;
                }
                case SFSpeechRecognizerAuthorizationStatusDenied: {
					NSLog(@"User denied access to speech recognition");
                    break;
                }
                case SFSpeechRecognizerAuthorizationStatusRestricted: {
					NSLog(@"User denied access to speech recognition");
                    break;
                }
                case SFSpeechRecognizerAuthorizationStatusNotDetermined: {
					NSLog(@"User denied access to speech recognition");
                    break;
                }
            }
        });
        
    }];
	
	return self;
}

- (void)SettingSpeech: (const char *) _language 
{	
    LanguageCode = [NSString stringWithUTF8String:_language];
    NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
    speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
    UnitySendMessage("SpeechToText", "onMessage", "Setting Success");
}
// recording
- (void)startRecording {
    if (!audioEngine.isRunning) {
        [inputNode removeTapOnBus:0];
        if (recognitionTask) {
            [recognitionTask cancel];
            recognitionTask = nil;
        }
        		
        AVAudioSession *session = [AVAudioSession sharedInstance];
        [session setCategory:AVAudioSessionCategoryPlayAndRecord withOptions:AVAudioSessionCategoryOptionDefaultToSpeaker|AVAudioSessionCategoryOptionMixWithOthers error:nil];
        [session setActive:TRUE error:nil];
        
        inputNode = audioEngine.inputNode;
        
        recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
        recognitionRequest.shouldReportPartialResults = YES;
        recognitionTask =[speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error)
        {
            if (result) {
                NSString *transcriptText = result.bestTranscription.formattedString;
                NSLog(@"STARTRECORDING RESULT: %@", transcriptText);
                if (result.isFinal) {
                    UnitySendMessage("SpeechToText", "onResults", [transcriptText UTF8String]);
                }
            }
            else {
                [audioEngine stop];
                recognitionTask = nil;
                recognitionRequest = nil;
                UnitySendMessage("SpeechToText", "onResults", "nil");
                NSLog(@"STARTRECORDING RESULT NULL");
            }
        }];

        AVAudioFormat *format = [inputNode outputFormatForBus:0];
        
        [inputNode installTapOnBus:0 bufferSize:1024 format:format block:^(AVAudioPCMBuffer * _Nonnull buffer, AVAudioTime * _Nonnull when) {
            [recognitionRequest appendAudioPCMBuffer:buffer];
        }];
        [audioEngine prepare];
        NSError *error1;
        [audioEngine startAndReturnError:&error1];

        if (error1.description) {
            NSLog(@"errorAudioEngine.description: %@", error1.description);
        }
    }
}

- (void)stopRecording {
    if (audioEngine.isRunning) {
        [inputNode removeTapOnBus:0];
		[audioEngine stop];
        [recognitionRequest endAudio];
    }
}

@end
extern "C"{
    SpeechRecorderViewController *vc = nil;
    
    SpeechRecorderViewController *getVc() {
        if (vc == nil) {
            vc = [[SpeechRecorderViewController alloc] init];
        }
        
        return vc;
    }
    
    void _TAG_startRecording(){
        SpeechRecorderViewController *pVc = getVc();
        [pVc startRecording];
    }
    void _TAG_stopRecording(){
        SpeechRecorderViewController *pVc = getVc();
        [pVc stopRecording];
    }
    void _TAG_SettingSpeech(const char * _language){
        SpeechRecorderViewController *pVc = getVc();
        [pVc SettingSpeech:_language];
    }
}
