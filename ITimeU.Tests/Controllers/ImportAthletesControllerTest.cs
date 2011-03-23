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
    ///     - Test that the user can upload a file, and the save them to the database.
    ///     - The_Controller_Must_Give_An_Error_If_Empty_File_Is_Uploaded_By_The_View: This was deleted
    ///     because I wasn't able to mock the Server object used by ImportAthletesController class.
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
                SendEmptyUploadRequestTo(importer);
                viewResult = (ViewResult)(importer.ImportFromFriRes());
            });

            Then("the controller should return a view with an error message", () =>
            {
                string error = viewResult.ViewBag.Error;
                error.ShouldBe(ImportAthletesController.ERROR_NO_FILE_UPLOADED);
            });
        }

        private void SendEmptyUploadRequestTo(ImportAthletesController importCtrl)
        {
            Mock<ControllerContext> controllerContext = createFakeControllerContext();
            importCtrl.ControllerContext = controllerContext.Object;
        }

        private Mock<ControllerContext> createFakeControllerContext()
        {
            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(d => d.HttpContext.Request.Files.Count).Returns(0);

            return controllerContext;
        }

    }
}
