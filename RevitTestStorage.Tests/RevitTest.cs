using Autodesk.Revit.ApplicationServices;
using NUnit.Framework;
using System;

namespace RevitTestStorage.Tests
{
    public class RevitTests
    {
        [Test]
        public void Application(Application application)
        {
            if (application.ActiveAddInId is null)
                Assert.Ignore("ActiveAddInId is no available");

            Console.WriteLine(application.ActiveAddInId.GetAddInName());
            Console.WriteLine(application.ActiveAddInId.GetGUID());
        }
    }
}
