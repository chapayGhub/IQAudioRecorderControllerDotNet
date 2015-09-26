using System;

namespace IQAudioRecorderController {
    
    
    public interface IIQAudioRecorderControllerDelegate {
        
        void AudioRecorderController(IQAudioRecorderController controller, String filePath);
        
        void AudioRecorderControllerDidCancel(IQAudioRecorderController controller);
    }
}
