using System;
using System.Threading.Tasks;
using DataAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HomeTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestGetHome()
        {
            HomeController hc = new HomeController();
            ContentResult res = hc.GetHome();

            Console.WriteLine(res.ContentType);
            Console.WriteLine(res.Content);
        }
    }
}