using System;

namespace IQAudioRecorderController {
    
    
    public interface IQAudioRecorderControllerDelegate {
        
        void AudioRecorderController(IQAudioRecorderController controller, String filePath);
        
        void AudioRecorderControllerDidCancel(IQAudioRecorderController controller);
    }
}
