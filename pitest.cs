using System;


using OSIsoft.AF.PI;


namespace PiUtinity
{
    class pitest   
    {
        public void test()
        { 
            PIServers piservers = new PIServers();
            foreach (var server in piservers)
            {
                Console.WriteLine("server :{0}", server.Name);
            }

            PIServer piserver = piservers.DefaultPIServer;
            Console.WriteLine("default server : {0}", piserver.Name);

            piserver.Connect();
            
            var point = PIPoint.FindPIPoint(piserver, "CD158");
            var value = point.Snapshot();
            Console.WriteLine("point {0} value {1} {2}", point.Name, value.Value.ToString(), value.Timestamp.ToString());


            piserver.Disconnect();

            Console.ReadKey();

          //  var point = PIPoint
        }

    }
}
