using AqbaServer.Helper;
using Newtonsoft.Json;
using System.Text;

namespace AqbaServer.API
{
    public static class TelegramNotification
    {
        public static async Task SendMessage(long? chatId, string content)
        {
            if (string.IsNullOrEmpty(content) || chatId == null) return;
            string json = string.Empty;

            try
            {
                json = JsonConvert.SerializeObject(content);
            }
            catch (Exception ex) { WriteLog.Error(ex.ToString()); return; }

            await Request.SendPostRequest(Config.TelegramBotApiLink + $"?chatId={chatId}", new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}
