using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System.Data.OleDb;
using System.Data;

namespace ITimeU.Tests.Library
{
    [TestClass]
    public class ImporterTest
    {
        [TestMethod]
        public void Test_Import_Participants_From_Access()
        {
            var importer = new Importer("ImporterTest.mdb");
            //importer
        }

    }
}
