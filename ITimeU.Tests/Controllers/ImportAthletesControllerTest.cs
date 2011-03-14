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

            When("we somehow send an empty upload request to the server", () =>
            {
                SendFakeUploadRequestTo(importer, 0);
                viewResult = (ViewResult)(importer.ImportFromFriRes());
            });

            Then("the controller should return a view with an error message", () =>
            {
                string error = viewResult.ViewBag.Error;
                error.ShouldBe(ImportAthletesController.ERROR_NO_FILE_UPLOADED);
            });
        }

        private void SendFakeUploadRequestTo(ImportAthletesController importCtrl, int fileCount)
        {
            Mock<ControllerContext> controllerContext = createFakeControllerContext(fileCount);
            importCtrl.ControllerContext = controllerContext.Object;
        }

        private Mock<ControllerContext> createFakeControllerContext(int fileCount)
        {
            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(d => d.HttpContext.Request.Files.Count).Returns(fileCount);
            if (fileCount > 0)
                putFilesInRequest(fileCount, controllerContext);

            return controllerContext;
        }

        private void putFilesInRequest(int fileCount, Mock<ControllerContext> controllerContext)
        {
            Mock<HttpPostedFileBase> file = new Mock<HttpPostedFileBase>();
            file.Setup(d => d.FileName).Returns("ImporterTest.mdb");
            file.Setup(d => d.InputStream).Returns(
                new FileStream(FriResImporterTest.DB_FILE, FileMode.Open));

            for (int i = 0; i < fileCount; i++)
                controllerContext.Setup(d => d.HttpContext.Request.Files[i]).Returns(file.Object);
        }

        [TestMethod]
        public void The_Controller_Must_Give_An_Error_If_Empty_File_Is_Uploaded_By_The_View()
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
                     SendFakeUploadRequestTo(importer, 1);
                     viewResult = (ViewResult)(importer.ImportFromFriRes());
                 });

            Then("the controller should return a view with an error message", () =>
            {
                string error = viewResult.ViewBag.Error;
                error.ShouldBe(ImportAthletesController.ERROR_NO_FILE_UPLOADED);
            });
        }

    }
}
