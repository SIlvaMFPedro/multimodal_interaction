using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace kinectModality
{
    public class SwipeLeftGesture
    {
        readonly int WINDOW_SIZE = 5;
        GestureSegment_Int1[] _gesture_segments;
        int _current_gesture_segment = 0;
        int _frame_count = 0;

        public event EventHandler GestureRecognized;

        public SwipeLeftGesture()
        {
            SwipeLeftSegment1 swipe_right_segment_1 = new SwipeLeftSegment1();
            SwipeLeftSegment2 swipe_right_segment_2 = new SwipeLeftSegment2();
            SwipeLeftSegment3 swipe_right_segment_3 = new SwipeLeftSegment3();

            _gesture_segments = new GestureSegment_Int1[] { swipe_right_segment_1, swipe_right_segment_2, swipe_right_segment_3 };
        }

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">Body Data.</param>
        public void Update(Body body)
        {
            GesturePartResult gesture_result = _gesture_segments[_current_gesture_segment].Update(body);

            if (gesture_result == GesturePartResult.Success)
            {
                if (_current_gesture_segment + 1 < _gesture_segments.Length)
                {
                    _current_gesture_segment++;
                    _frame_count = 0;
                }
                else
                {
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new EventArgs());
                        Reset();
                    }
                }
            }
            else if (_frame_count == WINDOW_SIZE)
            {
                Reset();
            }
            else
            {
                _frame_count++;
            }
        }

        /// <summary>
        /// Resets the current gesture.
        /// </summary>
        public void Reset()
        {
            _current_gesture_segment = 0;
            _frame_count = 0;
        }
    }
}
