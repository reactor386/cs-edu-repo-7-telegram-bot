//-
using System;
using System.Linq;


namespace WindWhisperBot.Utilities;

public static class NumCalc
{
    internal static string Calculate(string? str)    
    {
        string res;

        if (string.IsNullOrWhiteSpace(str))
        {
            res = "{пусто}";
        }
        else
        {
            decimal calcResult = 0;
            string[] numbers = str.Split(' ');
            foreach (string s in numbers)
            {
                if (decimal.TryParse(s, out decimal n))
                    calcResult += n;
            }
            res = $"сумма чисел - {calcResult}";
        }

        return res;
    }

}
