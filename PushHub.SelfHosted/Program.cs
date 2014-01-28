namespace PushHub.SelfHosted
{
    using System;
    using Nancy.Hosting.Self;

    class Program
    {
        static void Main(string[] args)
        {
            var uri =
                new Uri("http://localhost:4567");

            //Please either enable UrlReservations.CreateAutomatically on the HostConfiguration provided to 
            //the NancyHost, or create the reservations manually with the (elevated) command(s):
            
            using (var host = new NancyHost(
                new HostConfiguration {UrlReservations = new UrlReservations {CreateAutomatically = true}}, uri))
            {
                host.Start();

                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }
    }
}
