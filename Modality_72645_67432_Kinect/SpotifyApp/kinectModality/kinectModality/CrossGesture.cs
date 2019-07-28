using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Tools;
using Microsoft.Kinect.VisualGestureBuilder;
using Microsoft.Kinect.Wpf.Controls;

namespace kinectModality
{
    public class CrossGesture
    {
        readonly int WINDOW_SIZE = 50;
        GestureSegment_Int1[] _gesture_segments;
        int _current_gesture_segment = 0;
        int _frame_count = 0;

        public event EventHandler GestureRecognized;

        public CrossGesture()
        {
            CrossSegment cross_segment = new CrossSegment();

            _gesture_segments = new GestureSegment_Int1[] { cross_segment, cross_segment, cross_segment, cross_segment, cross_segment, cross_segment, cross_segment, cross_segment, cross_segment, cross_segment };
        }

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body data.</param>
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
            else if (gesture_result == GesturePartResult.Fail || _frame_count == WINDOW_SIZE)
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
