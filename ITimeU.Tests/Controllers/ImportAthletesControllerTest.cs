using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Controllers;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using ITimeU.Models;
using System.Web;
using Moq;
using System.IO;
using ITimeU.Tests.Library;
using System.Web.Routing;

namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// TODO:
    /// - Test that it is possible to upload files with the controller.
    /// </summary>
    [TestClass]
    public class ImportAthletesControllerTest : ScenarioClass
    {
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void Database_File_Exists()
        {
            System.IO.File.Exists(FriResImporterTest.DB_FILE).ShouldBeTrue();
        }

        [TestMethod]
        public void The_Controller_Must_Give_An_Error_If_No_File_Is_Uploaded_By_The_View()
        {
            ImportAthletesController importer = null;
            ViewResult viewResult = null;

            Given("we have an ImportAthletesController", () =>
            {
                importer = new ImportAthletesController();
            });

            When("we initiate the upload action (i.e. the users selects no file" +
                 "and clicks the submit button)", () =>
            {
                SendEmptyFakeUploadRequestTo(importer);
                viewResult = (ViewResult)(importer.ImportFromFriRes());
            });

            Then("the controller should return a view with an error message", () =>
            {
                string error = viewResult.ViewBag.Error;
                error.ShouldBe(ImportAthletesController.ERROR_NO_FILE_UPLOADED);
            });
        }

        private void SendEmptyFakeUploadRequestTo(ImportAthletesController importCtrl)
        {
            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(d => d.HttpContext.Request.Files.Count).Returns(0);
            importCtrl.ControllerContext = controllerContext.Object;
        }

    }
}
