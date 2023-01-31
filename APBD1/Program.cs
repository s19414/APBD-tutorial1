using System.Collections;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            string address = "";
            if(args.Length == 0 ) {
                httpClient.Dispose();
                throw new ArgumentNullException("argument cannot be null!");
            }
            address = args[0];
            Uri url;

            if (!Uri.TryCreate(address, UriKind.Absolute,out url) || address.Substring(0,4) != "http"){
                httpClient.Dispose();
                throw new ArgumentException("Argument must be valid url!");
            }

            HttpResponseMessage response = await httpClient.GetAsync(url);
            
            if(response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                char[] delimiterChars = { '<', '>', ' ' };
                string[] splitResult = result.Split(delimiterChars);
                List<string> foundEmails = new List<string>();
                bool emailAddressFound = false;
                foreach(string s in splitResult)
                {
                    //Console.WriteLine(s);
                    MailAddress mailAddress;
                    if (MailAddress.TryCreate(s, out mailAddress)){
                        foundEmails.Add(mailAddress.Address);
                        emailAddressFound = true;
                    }
                }
                if (!emailAddressFound) {
                    Console.WriteLine("E-mail addresses not found");
                }
                else
                {
                    
                    foreach(string email in foundEmails.Distinct())
                    {
                        Console.WriteLine(email);
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Error while downloading the page");
            }
            response.Dispose();
            httpClient.Dispose();
        }
    }
}
