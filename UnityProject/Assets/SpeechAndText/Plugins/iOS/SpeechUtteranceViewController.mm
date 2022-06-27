#import "SpeechUtteranceViewController.h"
#import "AVFoundation/AVFoundation.h"

@interface SpeechUtteranceViewController () <AVSpeechSynthesizerDelegate>
{
    NSString * speakText;
    NSString * LanguageCode;
    float pitch;
    float rate;
}
@property (nonatomic, retain) AVSpeechSynthesizer *speechSynthesizer;
@end

@implementation SpeechUtteranceViewController

- (id)init
{
    self = [super init];
    self.speechSynthesizer = [[AVSpeechSynthesizer alloc] init];
    self.speechSynthesizer.delegate = self;
    return self;
}
- (void)SettingSpeak: (const char *) _language pitchSpeak: (float)_pitch rateSpeak:(float)_rate
{
    LanguageCode = [NSString stringWithUTF8String:_language];
    pitch = _pitch;
    rate = _rate;
    UnitySendMessage("TextToSpeech", "onMessage", "Setting Success");
}
- (void)StartSpeak: (const char *) _text
{
    if([self.speechSynthesizer isSpeaking] == false) {
        speakText = [NSString stringWithUTF8String:_text];
        NSLog(@"%@", speakText);
        AVSpeechUtterance *utterance = [[AVSpeechUtterance alloc] initWithString:speakText];
        utterance.voice = [AVSpeechSynthesisVoice voiceWithLanguage:LanguageCode];
        utterance.pitchMultiplier = pitch;
        utterance.rate = rate;
        utterance.preUtteranceDelay = 0.2f;
        utterance.postUtteranceDelay = 0.2f;

        [self.speechSynthesizer speakUtterance:utterance];
    }
}
- (void)StopSpeak
{
    if([self.speechSynthesizer isSpeaking]) {
        [self.speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
    }
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
willSpeakRangeOfSpeechString:(NSRange)characterRange
                utterance:(AVSpeechUtterance *)utterance
{
    NSString *subString = [speakText substringWithRange:characterRange];
    UnitySendMessage("TextToSpeech", "onSpeechRange", [subString UTF8String]);
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
didStartSpeechUtterance:(AVSpeechUtterance *)utterance
{
    UnitySendMessage("TextToSpeech", "onStart", "onStart");
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
 didFinishSpeechUtterance:(AVSpeechUtterance *)utterance
{
    UnitySendMessage("TextToSpeech", "onDone", "onDone");
}

@end

extern "C"{

    SpeechUtteranceViewController *su = nil;

    SpeechUtteranceViewController * getSu() {
        if (su == nil) {
            su = [[SpeechUtteranceViewController alloc] init];
        }
        
        return su;
    }

    void _TAG_StartSpeak(const char * _text){
        SpeechUtteranceViewController *pSu = getSu();
        [pSu StartSpeak:_text];
    }
    void _TAG_StopSpeak(){
        SpeechUtteranceViewController *pSu = getSu();
        [pSu StopSpeak];
    }
    void _TAG_SettingSpeak(const char * _language, float _pitch, float _rate){
        SpeechUtteranceViewController *pSu = getSu();
        [pSu SettingSpeak:_language pitchSpeak:_pitch rateSpeak:_rate];
    }
}
