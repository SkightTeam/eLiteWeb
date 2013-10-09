using System;
using System.Runtime.InteropServices;

namespace FluentBuild.Utilities
{
    /// <summary>
    /// Sets the color of the console to support colorized output
    /// </summary>
    public class ConsoleColor
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

//        1 = Dark Blue
//2 = Dark Green
//3 = Aqua
//4 = Red
//5 = Purple
//6 = dark yellow
//7 = white
//8 - gray
//9 = blue
//10 = bright green
//11 = bright aqua
//12 = Bright Red
//13 = Bright purpole
//14 = Yellow
//15 = BrightWhite

        public enum BuildColor
        {
            DarkBlue=1, 
            DarkGreen=2, 
            DarkAqua=3,
            DarkRed=4,
            DarkPurple=5,
            DarkYellow=6,
            White=007, 
            Gray = 8,
            BrightBlue =9,
            BrightGreen=10, 
            BrightAqua=11,
            BrightRed = 12, 
            BrightPurple=13,
            BrightYellow = 14,
            BrightWhite=15
        };

        public static void ShowAllColors()
        {
            IntPtr hConsole = GetStdHandle(0xfffffff5);
            for (int k = 1; k < 130; k++)
            {
                SetConsoleTextAttribute(hConsole, k);
                Console.WriteLine("{0:d3} I want to be nice today!", k);
            }

            //go back to the default
            SetConsoleTextAttribute(hConsole, 007);
        }

        public static void SetColor(BuildColor color)
        {
            //0xfffffff5 is the default handle for reasons unknown
            IntPtr hConsole = GetStdHandle(0xfffffff5);
            SetConsoleTextAttribute(hConsole, (int)color);
        }

        public static IDisposable SetTemporaryColor(BuildColor color)
        {
            SetColor(color);
            return new ReturnColorToDefault();
        }
    }

    internal class ReturnColorToDefault : IDisposable
    {
        public void Dispose()
        {
            ConsoleColor.SetColor(ConsoleColor.BuildColor.White);
        }
    }
}