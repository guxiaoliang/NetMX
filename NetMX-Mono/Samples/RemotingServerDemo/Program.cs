using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.Remote;
using NetMX.Relation;

namespace RemotingServerDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
         server.RegisterMBean(new RelationService(), RelationService.ObjectName);
			Sample sample1 = new Sample();
         Sample sample2 = new Sample();
         Sample sample3 = new Sample();			
			server.RegisterMBean(sample1, "Sample:type=Sample,id=1");
         server.RegisterMBean(sample2, "Sample:type=Sample,id=2");
         server.RegisterMBean(sample3, "Sample:type=Sample,id=3");

         RelationServiceMBean relationSerice = NetMX.NetMX.NewMBeanProxy<RelationServiceMBean>(server, RelationService.ObjectName);
         relationSerice.CreateRelationType("Binding", new RoleInfo[] {
            new RoleInfo("Source", typeof(SampleMBean), true, false, 1, 1, "Source"),
            new RoleInfo("Destination", typeof(SampleMBean), true, false, 1, 1, "Destination")});

         relationSerice.CreateRelation("Rel1", "Binding", new Role[] {
            new Role("Source", new ObjectName("Sample:type=Sample,id=1")),
            new Role("Destination", new ObjectName("Sample:type=Sample,id=2"))});

         relationSerice.CreateRelation("Rel2", "Binding", new Role[] {
            new Role("Source", new ObjectName("Sample:type=Sample,id=1")),
            new Role("Destination", new ObjectName("Sample:type=Sample,id=3"))});


			Uri serviceUrl = new Uri("tcp://localhost:1234/MBeanServer.tcp");

			using (INetMXConnectorServer connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
			{
				connectorServer.Start();
				Console.WriteLine("Press any key to quit");
				Console.ReadKey();
			}			
		}
	}
}