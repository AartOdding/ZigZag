using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZag.Mathematics
{
    public static class Utils
    {
        public static float MapRange(float value, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
        }

        public static Vector2 MapRange(Vector2 value, Vector2 inputMin, Vector2 inputMax, Vector2 outputMin, Vector2 outputMax)
        {
            return new Vector2(MapRange(value.X, inputMin.X, inputMax.X, outputMin.X, outputMax.X), 
                               MapRange(value.Y, inputMin.Y, inputMax.Y, outputMin.Y, outputMax.Y));
        }

        public static Vector3 MapRange(Vector3 value, Vector3 inputMin, Vector3 inputMax, Vector3 outputMin, Vector3 outputMax)
        {
            return new Vector3(MapRange(value.X, inputMin.X, inputMax.X, outputMin.X, outputMax.X),
                               MapRange(value.Y, inputMin.Y, inputMax.Y, outputMin.Y, outputMax.Y),
                               MapRange(value.Z, inputMin.Z, inputMax.Z, outputMin.Z, outputMax.Z));
        }
    }
}
