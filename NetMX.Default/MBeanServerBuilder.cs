#region USING
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace NetMX.Server
{
   public sealed class MBeanServerBuilder : NetMX.MBeanServerBuilder
   {		
      public override IMBeanServer NewMBeanServer(string instanceName)
      {
         return new MBeanServer(instanceName);
      }
   }
}