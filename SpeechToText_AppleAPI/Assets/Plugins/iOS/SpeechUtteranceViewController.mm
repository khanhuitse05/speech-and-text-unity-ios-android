#import "SpeechUtteranceViewController.h"
#import "AVFoundation/AVFoundation.h"

@interface SpeechUtteranceViewController () <AVSpeechSynthesizerDelegate>
{
	@property (readwrite, nonatomic, strong) AVSpeechSynthesizer *speechSynthesizer;
	NSString * speakText;
	NSString * LanguageCode=@"ko-KR";
	float pitch = 0.5f;
	float rate = 0.5f;
}
@end

@implementation SpeechUtteranceViewController

- (id)init
{
	self = [super init];
	return self;
}
- (void)InitSpeak
{	
	self.speechSynthesizer = [[AVSpeechSynthesizer alloc] init];
	self.speechSynthesizer.delegate = self;
}
- (void)SettingSpeak: (const char *) _language pitchSpeak: (float)_pitch rateSpeak:(float)_rate
{	
	LanguageCode = [NSString stringWithUTF8String:_language];
	pitch = _pitch;
	rate = _rate;
}
- (void)StartSpeak: (const char *) _text
{
    speakText = [NSString stringWithUTF8String:_text];
    AVSpeechUtterance *utterance = [[AVSpeechUtterance alloc] initWithString:speakText];
    utterance.voice = [AVSpeechSynthesisVoice voiceWithLanguage:LanguageCode];
    utterance.pitchMultiplier = pitch;
    utterance.rate = rate;
    utterance.preUtteranceDelay = 0.2f;
    utterance.postUtteranceDelay = 0.2f;

    [self.speechSynthesizer speakUtterance:utterance];
}
- (void)StopSpeak
{	
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
willSpeakRangeOfSpeechString:(NSRange)characterRange
                utterance:(AVSpeechUtterance *)utterance
{
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
  didStartSpeechUtterance:(AVSpeechUtterance *)utterance
{    
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
 didFinishSpeechUtterance:(AVSpeechUtterance *)utterance
{	
}

@end

extern "C"{
    SpeechUtteranceViewController *su = [[SpeechUtteranceViewController alloc] init];  
	void _TAG_StopSpeak(){
        [su InitSpeak];
    } 	
    void _TAG_StartSpeak(const char * _text){
        [su StartSpeak:_text];
    }
	void _TAG_StopSpeak(){
        [su StopSpeak];
    } 
	void _TAG_SettingSpeak(const char * _language, float _pitch, float _rate){
        [su SettingSpeak:_language pitchSpeak:_pitch rateSpeak:_rate];
    }    
}
