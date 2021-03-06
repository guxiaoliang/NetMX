using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;
using System.Security.Principal;

namespace NetMX.Tests
{
   /// <summary>
   /// Summary description for MBeanPermissionTest
   /// </summary>
   [TestFixture]
   public class MBeanPermissionTest
   {
      #region Additional test attributes
      //
      // You can use the following additional attributes as you write your tests:
      //
      // Use ClassInitialize to run code before running the first test in the class
      // [ClassInitialize()]
      // public static void MyClassInitialize(TestContext testContext) { }
      //
      // Use ClassCleanup to run code after all tests in a class have run
      // [ClassCleanup()]
      // public static void MyClassCleanup() { }
      //
      // Use TestInitialize to run code before running each test 
      // [TestInitialize()]
      // public void MyTestInitialize() { }
      //
      // Use TestCleanup to run code after each test has run
      // [TestCleanup()]
      // public void MyTestCleanup() { }
      //
      #endregion

      [Test]
      public void CopyTest()
      {
         MBeanPermission perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         MBeanPermission permCopy = (MBeanPermission)perm.Copy();
         Assert.IsTrue(perm.Equals(permCopy));
         Assert.IsTrue(permCopy.Equals(perm));
      }

      [Test]
      public void TextFormatTest()
      {
         MBeanPermission perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         MBeanPermission permText = new MBeanPermission("NetMX.StandardMBean, NetMX#getMBeanInfo[Sample:]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));

         perm = new MBeanPermission(null, "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         permText = new MBeanPermission("-#getMBeanInfo[Sample:]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", null, new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         permText = new MBeanPermission("NetMX.StandardMBean, NetMX#-[Sample:]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", null, MBeanPermissionAction.Invoke);
         permText = new MBeanPermission("NetMX.StandardMBean, NetMX#getMBeanInfo[-]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));

         perm = new MBeanPermission("", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         permText = new MBeanPermission("#getMBeanInfo[Sample:]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         permText = new MBeanPermission("NetMX.StandardMBean, NetMX#[Sample:]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName(":"), MBeanPermissionAction.Invoke);
         permText = new MBeanPermission("NetMX.StandardMBean, NetMX#getMBeanInfo[:]", MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(permText));
         Assert.IsTrue(permText.Equals(perm));
      }

      [Test]
      public void InSubsetOfTest()
      {
         MBeanPermission permFull = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.IsSubsetOf(permFull));

         MBeanPermission perm = new MBeanPermission(null, "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.IsSubsetOf(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", null, new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.IsSubsetOf(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", null, MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.IsSubsetOf(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke | MBeanPermissionAction.Instantiate);
         Assert.IsTrue(permFull.IsSubsetOf(perm));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Instantiate);
         Assert.IsFalse(permFull.IsSubsetOf(perm));
      }
      [Test]
      public void UnionTest()
      {
         MBeanPermission permFull = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);

         MBeanPermission perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(perm.Union(permFull)));
         Assert.IsTrue(perm.Equals(permFull.Union(perm)));

         perm = new MBeanPermission(null, "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(perm.Union(permFull)));
         Assert.IsTrue(perm.Equals(permFull.Union(perm)));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", null, new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(perm.Union(permFull)));
         Assert.IsTrue(perm.Equals(permFull.Union(perm)));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", null, MBeanPermissionAction.Invoke);
         Assert.IsTrue(perm.Equals(perm.Union(permFull)));
         Assert.IsTrue(perm.Equals(permFull.Union(perm)));

         perm = null;
         Assert.IsTrue(permFull.Equals(permFull.Union(perm)));
      }
      [Test]
      public void IntersectTest()
      {
         MBeanPermission permFull = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);

         MBeanPermission perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.Equals(perm.Intersect(permFull)));
         Assert.IsTrue(permFull.Equals(permFull.Intersect(perm)));

         perm = new MBeanPermission(null, "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.Equals(perm.Intersect(permFull)));
         Assert.IsTrue(permFull.Equals(permFull.Intersect(perm)));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", null, new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.Equals(perm.Intersect(permFull)));
         Assert.IsTrue(permFull.Equals(permFull.Intersect(perm)));

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", null, MBeanPermissionAction.Invoke);
         Assert.IsTrue(permFull.Equals(perm.Intersect(permFull)));
         Assert.IsTrue(permFull.Equals(permFull.Intersect(perm)));

         perm = null;
         Assert.IsNull(permFull.Intersect(perm));
      }
      [Test]
      public void DemandTest()
      {
         SetPermissions(new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke));
         MBeanPermission perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         perm.Demand();

         perm = new MBeanPermission(null, "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         perm.Demand();

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", null, new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         perm.Demand();

         perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", null, MBeanPermissionAction.Invoke);
         perm.Demand();
      }

      [Test]
      [ExpectedException(typeof(InvalidOperationException))]
      public void DemandVerifyAsNeededFailueTest()
      {
         SetPermissions(new MBeanPermission(null, "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke));
         MBeanPermission perm = new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         perm.Demand();
      }

      [Test]
      [ExpectedException(typeof(InvalidOperationException))]
      public void DemandVerifyAsHeldFailueTest()
      {
         SetPermissions(new MBeanPermission("NetMX.StandardMBean, NetMX", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke));
         MBeanPermission perm = new MBeanPermission("", "getMBeanInfo", new ObjectName("Sample:"), MBeanPermissionAction.Invoke);
         perm.Demand();
      }

      #region Utility
      private static void SetPermissions(params MBeanPermission[] permissions)
      {
         //Thread.SetData(Thread.GetNamedDataSlot("NetMX.MBeanPermission"), permissions);
         Thread.CurrentPrincipal = new DummyPrincipal(permissions);
      }
      private class DummyPrincipal : GenericPrincipal, INetMXPrincipal
      {
         private readonly IEnumerable<MBeanPermission> _permissions;

         public DummyPrincipal(IEnumerable<MBeanPermission> permissions)
            : base(new GenericIdentity("Name", "Type"),new string[] {})
         {            
            _permissions = permissions;
         }

         #region INetMXPrincipal Members
         public IEnumerable<MBeanPermission> Permissions
         {
            get { return _permissions; }
         }
         #endregion
      }
      #endregion
   }
}
