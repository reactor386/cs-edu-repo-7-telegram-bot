//-
using System;

using WindWhisperBot.Models;


namespace WindWhisperBot.Services;

public interface IStorage
{
    /// <summary>
    /// Получение сессии пользователя по идентификатору
    /// </summary>
    Session GetSession(long chatId);
}
