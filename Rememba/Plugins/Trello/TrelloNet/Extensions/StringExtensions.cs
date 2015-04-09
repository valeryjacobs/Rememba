namespace TrelloNet.Extensions
{
    public static class StringExtensions
    {
         public static string LowerFirst(this string input)
         {
             var result = input.Substring(0, 1).ToLower() + 
                 input.Substring(1, input.Length - 1);
             return result;
         }
    }
}