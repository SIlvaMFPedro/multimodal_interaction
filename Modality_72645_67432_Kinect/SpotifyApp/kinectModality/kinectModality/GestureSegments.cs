using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace kinectModality
{
    public class GestureSegments
    {
    }

    /// Represents a single gesture segment which uses relative positioning of body parts to detect a gesture.
    public interface GestureSegment_Int1
    {
        // <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        GesturePartResult Update(Body body);
    }

    /// Represents a single gesture segment which uses relative positioning of body parts to detect a gesture.
    public interface GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        GesturePartResult Update(Body body);
        float[] get();
        void set(float[] previous_position);
    }

    public class CloseHandSegment : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- OPEN RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        int count = 0;
        float previous_position;
        public GesturePartResult Update(Body body)
        {
            if(count == 0)
            {
                previous_position = body.Joints[JointType.HandRight].Position.Z;
                count++;
                HandState rightHand = body.HandRightState;
                if(rightHand == HandState.Closed) //check if hand is closed
                {
                    return GesturePartResult.Success;

                }
                else
                {
                    return GesturePartResult.Fail;
                }
            }
            else if(body.Joints[JointType.HandRight].Position.Z < previous_position - 0.025)
            {
                previous_position = body.Joints[JointType.HandRight].Position.Z;
                HandState rightHand = body.HandRightState;
                if(rightHand == HandState.Closed) //check if hand is closed;
                {
                    return GesturePartResult.Success;
                }
                else
                {
                    return GesturePartResult.Fail;
                }
            }
            //Hand dropped
            count = 0; //reset counter;
            return GesturePartResult.Fail;
        }
    }

    public class OpenHandSegment : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- OPEN RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        int count = 0;
        float previous_position;
        public GesturePartResult Update(Body body)
        {
            if(count == 0)
            {
                previous_position = body.Joints[JointType.HandRight].Position.Z;
                count++;
                return GesturePartResult.Success;
            }
            if(body.Joints[JointType.HandRight].Position.Z < previous_position - 0.025)
            {
                previous_position = body.Joints[JointType.HandRight].Position.Z;
                return GesturePartResult.Success;
            }
            //Hand dropped
            count = 0; //reset counter;
            return GesturePartResult.Fail;
        }
    }

    public class SwipeRightSegment1 : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        int count = 0;
        public GesturePartResult Update(Body body)
        {
            if(count == 0)
            {
                if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                {
                    count++;
                    return GesturePartResult.Success;
                }
            }
            // Hand dropped
            count = 0; //reset counter;
            return GesturePartResult.Fail;
        }
    }
     
    public class SwipeRightSegment2 : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
            {
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
    }

    public class SwipeRightSegment3 : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        public GesturePartResult Update(Body body)
        {

            if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
    }

    public class SwipeLeftSegment1 : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE LEFT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        int count = 0;
        public GesturePartResult Update(Body body)
        {
            if(count == 0)
            {
                if(body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                {
                    count++;
                    return GesturePartResult.Success;
                }
            }
            // Hand dropped
            count = 0; //reset counter;
            return GesturePartResult.Fail;
        }
    }

    public class SwipeLeftSegment2 : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE LEFT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X && body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
            {
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
    }

    public class SwipeLeftSegment3 : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE LEFT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
    }

    public class SwipeUpSegment : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        int count = 0;
        public GesturePartResult Update(Body body)
        {
            if (count == 0)
            {
                if (body.Joints[JointType.HandLeft].Position.Y >= body.Joints[JointType.ShoulderLeft].Position.Y)
                {
                    count++;
                    return GesturePartResult.Success;
                }
            }
            // Hand dropped
            count = 0; //reset counter;
            return GesturePartResult.Fail;
        }
    }

    public class SwipeDownSegment : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- SWIPE RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        int count = 0;
        public GesturePartResult Update(Body body)
        {
            if (count == 0)
            {
                if (body.Joints[JointType.HandLeft].Position.Y <= body.Joints[JointType.ShoulderLeft].Position.Y)
                {
                    count++;
                    return GesturePartResult.Success;
                }
            }
            // Hand dropped
            count = 0; //reset counter;
            return GesturePartResult.Fail;
        }
    }


    public class CrossSegment : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- CROSS ARMS.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
            {
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
                {
                    if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.HandRight].Position.X)
                    {
                        return GesturePartResult.Success;
                    }
                }
            }
            // Hand dropped
            return GesturePartResult.Fail;
        }
    }

    public class CloseEarsSegment : GestureSegment_Int1
    {
        /// <summary>
        /// Updates the current gesture -- CLOSE EARS.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ShoulderLeft].Position.Y)
            {
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ShoulderRight].Position.Y)
                {
                    return GesturePartResult.Success;
                }
            }
            // Hand dropped
            return GesturePartResult.Fail;
        }
    }

    public class RotateClockWiseSegment1 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateClockWiseSegment2 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X > previous_pos_x + 0.01 && body.Joints[JointType.HandRight].Position.Y < previous_pos_y - 0.01)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateClockWiseSegment3 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X < previous_pos_x - 0.01 && body.Joints[JointType.HandRight].Position.Y < previous_pos_y - 0.01)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateClockWiseSegment4 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X < previous_pos_x - 0.01 && body.Joints[JointType.HandRight].Position.Y > previous_pos_y + 0.01)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateCounterClockWiseSegment1 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE COUNTER CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateCounterClockWiseSegment2 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE COUNTER CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X < previous_pos_x + 0.01 && body.Joints[JointType.HandRight].Position.Y < previous_pos_y - 0.01)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateCounterClockWiseSegment3 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE COUNTER CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X > previous_pos_x - 0.01 && body.Joints[JointType.HandRight].Position.Y < previous_pos_y - 0.01)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

    public class RotateCounterClockWiseSegment4 : GestureSegment_Int2
    {
        /// <summary>
        /// Updates the current gesture -- ROTATE COUNTER CLOCKWISE -> RIGHT HAND.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>

        float previous_pos_x, previous_pos_y;
        public GesturePartResult Update(Body body)
        {
            if (body.Joints[JointType.HandRight].Position.X > previous_pos_x - 0.01 && body.Joints[JointType.HandRight].Position.Y > previous_pos_y + 0.01)
            {
                previous_pos_x = body.Joints[JointType.HandRight].Position.X;
                previous_pos_y = body.Joints[JointType.HandRight].Position.Y;
                return GesturePartResult.Success;
            }
            return GesturePartResult.Fail;
        }
        public void set(float[] previous_position)
        {
            this.previous_pos_x = previous_position[0];
            this.previous_pos_y = previous_position[1];
        }
        public float[] get()
        {
            float[] previous_position = { this.previous_pos_x, this.previous_pos_y };
            return previous_position;
        }
    }

}
