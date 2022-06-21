using System;


namespace Utils.Time
{
    
    public static class Format
    {

        public static string GetTimeNow()
        {
            DateTime now = DateTime.Now;
            return $"{now:HH:mm:ss}";
        }

    }

}