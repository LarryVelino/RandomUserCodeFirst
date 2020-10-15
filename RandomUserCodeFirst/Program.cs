namespace RandomUserCodeFirst
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Xml;

    using RandomUserCodeFirst.Model;

    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static void ClearDatabase(UserContext userContext)
        {
            userContext.Database.EnsureDeleted();
        }

        private static User ConvertXmlToUser(XmlDocument xml)
        {
            var fullName = ParseXmlToCreateFullName(xml);
            var email = ParseXmlForEmail(xml);
            var phone = ParseXmlForPhone(xml);
            var cell = ParseXmlForCell(xml);

            return new User { Cell = cell, Name = fullName, Email = email, Phone = phone };
        }

        private static async Task GetAndStoreUserInContext(UserContext userContext, int numUsers)
        {
            for (var x = 0; x < numUsers; x++)
            {
                var user = await RunGetUserAsync();
                StoreUserInContextIfNotNull(userContext, user);
            }

            SaveChangesToDatabase(userContext);
        }

        private static async Task<User> GetUserAsync(string path)
        {
            var xml = new XmlDocument();

            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                await ProcessResponseToXml(response, xml);

                return ConvertXmlToUser(xml);
            }

            return null;
        }

        private static void InitClient()
        {
            client.BaseAddress = new Uri("http://localhost:55268/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        }

        private static void Main(string[] args)
        {
            InitClient();
            using (var userContext = new UserContext())
            {
                Console.WriteLine("Inserting seed User");
                userContext.Add(new User { Name = "LRV", Cell = "555", Email = "@.com", Phone = "555" });
                userContext.SaveChanges();

                Console.WriteLine("User Retrieval In Progress... ");
                GetAndStoreUserInContext(userContext, 5).Wait();
                Console.WriteLine($"User Retrieval Complete! {Environment.NewLine}");
                WriteUsersToConsole(userContext);
                ClearDatabase(userContext);
            }

            Console.ReadLine();
        }

        private static string ParseXmlForCell(XmlDocument xml)
        {
            var cell = xml.SelectSingleNode("//user/results/cell")?.InnerText;
            return cell;
        }

        private static string ParseXmlForEmail(XmlDocument xml)
        {
            var email = xml.SelectSingleNode("//user/results/email")?.InnerText;
            return email;
        }

        private static string ParseXmlForPhone(XmlDocument xml)
        {
            var phone = xml.SelectSingleNode("//user/results/phone")?.InnerText;
            return phone;
        }

        private static string ParseXmlToCreateFullName(XmlDocument xml)
        {
            var title = xml?.SelectSingleNode("//user/results/name/title")?.InnerText;
            var first = xml?.SelectSingleNode("//user/results/name/first")?.InnerText;
            var last = xml?.SelectSingleNode("//user/results/name/last")?.InnerText;
            return $"{title} {first} {last}";
        }

        private static async Task ProcessResponseToXml(HttpResponseMessage response, XmlDocument xml)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            xml.LoadXml(responseString);
        }

        private static async Task<User> RunGetUserAsync()
        {
            Console.WriteLine("Getting User");
            var user = await GetUserAsync("https://randomuser.me/api/?format=xml");
            Console.WriteLine("Retrieved Result for User");
            return user;
        }

        private static void SaveChangesToDatabase(UserContext userContext)
        {
            userContext.SaveChanges();
        }

        private static void StoreUserInContextIfNotNull(UserContext userContext, User user)
        {
            if (user != null)
            {
                Console.WriteLine("Storing Valid User in Context");
                userContext.Users.Add(user);
            }
        }

        private static void WriteAllUsersToConsole(IOrderedQueryable<User> query)
        {
            foreach (var item in query)
            {
                Console.WriteLine($"User Id: {item.UserId}");
                Console.WriteLine($"User Name: {item.Name}");
                Console.WriteLine($"User Email: {item.Email}");
                Console.WriteLine($"User Phone: {item.Phone}");
                Console.WriteLine($"User Cell: {item.Cell}{Environment.NewLine}");
            }
        }

        private static void WriteUsersToConsole(UserContext userContext)
        {
            var query = from user in userContext.Users orderby user.UserId select user;
            WriteAllUsersToConsole(query);
        }
    }
}