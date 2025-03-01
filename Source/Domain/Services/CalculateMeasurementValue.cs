﻿namespace Domain
{
    public static class CalculateMeasurementValue
    {
        public static double ForRangeValueUsingMultipliers(
            double rangeValue,
            double impedanceMultiplier,
            double interferenceMultiplier)
        {
            return rangeValue + interferenceMultiplier * impedanceMultiplier * rangeValue;
        }
    }
}