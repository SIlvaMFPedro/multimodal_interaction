using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace kinectModality
{
    public class RotateCounterClockWiseGesture
    {
        readonly int WINDOW_SIZE = 5;
        GestureSegment_Int2[] _gesture_segments;
        int _current_gesture_segment = 0;
        int _frame_count = 0;

        float previous_pos_x, previous_pos_y;

        public event EventHandler GestureRecognized;

        public RotateCounterClockWiseGesture()
        {
            RotateCounterClockWiseSegment1 rotation_counter_clockwise_segment_1 = new RotateCounterClockWiseSegment1();
            RotateCounterClockWiseSegment2 rotation_counter_clockwise_segment_2 = new RotateCounterClockWiseSegment2();
            RotateCounterClockWiseSegment3 rotation_counter_clockwise_segment_3 = new RotateCounterClockWiseSegment3();
            RotateCounterClockWiseSegment4 rotation_counter_clockwise_segment_4 = new RotateCounterClockWiseSegment4();

            _gesture_segments = new GestureSegment_Int2[] { rotation_counter_clockwise_segment_1, rotation_counter_clockwise_segment_2, rotation_counter_clockwise_segment_3, rotation_counter_clockwise_segment_4 };
        }

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">Body Data.</param>
        public void Update(Body body)
        {
            GesturePartResult result = _gesture_segments[_current_gesture_segment].Update(body);

            if (result == GesturePartResult.Success)
            {
                if (_current_gesture_segment + 1 < _gesture_segments.Length)
                {
                    _gesture_segments[_current_gesture_segment + 1].set(_gesture_segments[_current_gesture_segment].get());
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
