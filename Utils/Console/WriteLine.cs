using System;
using Utils.Time;


namespace Utils.Console
{
    
    public static class WriteLine
    {

        public static void Log(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.White;

            WriteLine.Write(message);
        }

        public static void LogWarning(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            
            WriteLine.Write(message);
        }

        public static void LogError(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            
            WriteLine.Write(message);
        }


        private static void Write(string message)
        {
            string time = Time.Format.GetTimeNow();

            System.Console.WriteLine($"[{time}]: {message}");
        }

    }

}