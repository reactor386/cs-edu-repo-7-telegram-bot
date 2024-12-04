//-
using System;
using System.Linq;


namespace WindWhisperBot.Utilities;

public static class TxtCalc
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
            res = $"в вашем сообщении символов - {str.ToCharArray().Length}";
        }

        return res;
    }
}
