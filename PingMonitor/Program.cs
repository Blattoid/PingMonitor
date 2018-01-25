using System;
using System.Threading;
using System.Net.NetworkInformation;

namespace PingMonitor
{
    class Program
    {
        //Defines isPingable, which takes a string for the address. It returns a boolean for if it is pingable or not.
        static bool isPingable(string address)
        {
            //Set pingable to false. If the host isn't pingable, this won't change.
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                //Attempt to ping the host
                PingReply reply = pinger.Send(address);
                //Set pingable to the ping reply status, either True or False.
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException) { /* Discard any errors */ }
            return pingable;
        }
        static void Main(string[] args)
        {
            string addr,time;
            // if there is no argument, specify there must be an argument and close. otherwise use the argument at the port number.
            if (args.Length >= 1) //only one argument will do, so any other arguments are ignored.
            {
                addr = args[0]; //set the target to the first argument
            }
            else
            {
                Console.WriteLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " <target>"); //Remind them of the syntax.
                Console.Write("Enter address: ");
                addr = Console.ReadLine();
            }
            Console.WriteLine("Started logging status of "+addr+" at "+ DateTime.Now.ToString());

            bool pre_ping = isPingable(addr); //set a baseline
            if (pre_ping)
            {
                Console.WriteLine("Host is currently online.");
            }
            else
            {
                Console.WriteLine("Host is currently offline.");
            }
            for (; ; )
            {
                Thread.Sleep(1000); //Wait 1 second before pinging again to prevent ping spam blocking.
                bool pingable = isPingable(addr); //determine if the host is pingable
                time = DateTime.Now.ToString(); //get the time and date

                //logic for determining if host went online/offline
                if (!(pingable == pre_ping))
                {
                    pre_ping = pingable;
                    if (pingable)
                    {
                        Console.WriteLine(addr + " came online at " + time);
                    }
                    else
                    {
                        Console.WriteLine(addr + " went offline at " + time);
                    }
                }
            }
        }
    }
}
