namespace LostAngeles.Shared
{
    public class Math
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }

            if (value < min)
            {
                return min;
            }
            
            return value;
        }

        public static float MapRange(float value, float inRangeMin, float inRangeMax, float outRangeMin, float outRangeMax)
        {
            if (value < inRangeMin) value = inRangeMin;
            if (value > inRangeMax) value = inRangeMax;
            return (value - inRangeMin) / (inRangeMax - inRangeMin) * (outRangeMax - outRangeMin) + outRangeMin; 
        } 
    }
}
