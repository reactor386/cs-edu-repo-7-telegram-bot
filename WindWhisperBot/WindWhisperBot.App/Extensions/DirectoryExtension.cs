//-
using System;
using System.IO;

using System.Reflection;


namespace WindWhisperBot.Extensions;

public static class DirectoryExtension
{
    /// <summary>
    /// Получаем путь до каталога, в который вложен каталог с библиотекой
    /// </summary>
    public static string GetRootForLibrary(string libraryFolderName)
    {
        string rootLocation = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        while (!string.IsNullOrWhiteSpace(rootLocation))
        {
            if (Directory.GetDirectories(rootLocation, libraryFolderName,
                SearchOption.TopDirectoryOnly).Length > 0)
            {
                break;
            }
            rootLocation = Path.GetDirectoryName(rootLocation) ?? string.Empty;
        }

        return rootLocation;
    }
}
