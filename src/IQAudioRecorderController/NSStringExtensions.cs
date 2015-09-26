using System;
using Foundation;

namespace IQAudioRecorderController {
    
    
    #region 
	static
    internal class NSStringExtensions {
        
        #region Static Methods
        public static String TimeStringForTimeInterval(double timeInterval) {
            // 
            // {
            //     NSInteger ti = (NSInteger)timeInterval;
            //     NSInteger seconds = ti % 60;
            //     NSInteger minutes = (ti / 60) % 60;
            //     NSInteger hours = (ti / 3600);
            //     
            //     if (hours > 0)
            //     {
            //         return [NSString stringWithFormat:@"%02li:%02li:%02li", (long)hours, (long)minutes, (long)seconds];
            //     }
            //     else
            //     {
            //         return  [NSString stringWithFormat:@"%02li:%02li", (long)minutes, (long)seconds];
            //     }
            // }

			return String.Empty;
        }
        #endregion
    }
    #endregion
}
