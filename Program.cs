using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Servers
    {
        private static readonly Servers instance = new Servers();
        private readonly List<string> servers;
        private static readonly object lockObject = new object();

        private Servers()
        {
            servers = new List<string>();
        }

        public static Servers Instance { get { return instance; } }

        public bool AddServer(string server)
        {
            lock (lockObject)
            {
                if (!server.StartsWith("http://") && !server.StartsWith("https://"))
                {
                    return false;
                }
                if (servers.Contains(server))
                {
                    return false;
                }
                servers.Add(server);
                return true;
            }
        }

        public List<string> GetHttpServers()
        {
            lock (lockObject)
            {
                return servers.Where(s => s.StartsWith("http://")).ToList();
            }
        }

        public List<string> GetHttpsServers()
        {
            lock (lockObject)
            {
                return servers.Where(s => s.StartsWith("https://")).ToList();
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Servers servers = Servers.Instance;
            Console.WriteLine(servers.AddServer("https://test.com"));
            Console.WriteLine(servers.AddServer("https://example.com"));
            Console.WriteLine(servers.AddServer("https://example.org"));

            Console.WriteLine(servers.AddServer("https://example.org"));

            List<string> httpServers = servers.GetHttpServers();
            List<string> httpsServers = servers.GetHttpsServers();

            foreach (var server in httpsServers)
            {
                Console.WriteLine(server);
                
            }
        }
    }
}