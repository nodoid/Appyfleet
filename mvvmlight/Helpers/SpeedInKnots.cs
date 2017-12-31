namespace mvvmframework.Helpers
{
    public static class SpeedInKnots
    {
        public static double KmhToKnots(this double speed)
        {
            return speed * .54;
        }
    }
}
