using System.Text;

namespace AqbaServer.Authorization
{
    public class RandomString
    {
        const string UPALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string LOWALPHABET = "abcdefghijklmnopqrstuvwxyz";
        const string NUMBERS = "1234567890";
        const string SYMBOLS = "!@#%*()-=";
        const string CHARTS = UPALPHABET + LOWALPHABET + NUMBERS + SYMBOLS;
        static readonly Random rand = new ();
        public static string GetString(int length)
        {            
            StringBuilder sb = new(length - 1);
            for (int i = 0; i < length; i++)
            {
                sb.Append( CHARTS[ rand.Next(0, CHARTS.Length - 1) ] );
            }
            return sb.ToString();
        }
    }
}
