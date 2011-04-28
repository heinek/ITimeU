using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class AthleteController : Controller
    {
        //
        // GET: /Athlete/

        public ActionResult Index()
        {
            setViewData();
            ViewBag.IsAthleteCreated = false;
            ViewBag.IsValidInput = true;
            ViewBag.IsStartNumberExist = false;
            return View(ClubModel.GetAll());
        }

        public ActionResult Setup()
        {
            return View();
        }

        private void setViewData()
        {
            ViewBag.Gender = getGender();
            ViewBag.BirthDate = getBirthDate();
            ViewBag.AthleteClass = AthleteClassModel.GetAll();

        }

        private void setEditViewData()
        {
            ViewBag.Gender = getGender();
            ViewBag.BirthDate = getBirthDate();
            ViewBag.AthleteClass = AthleteClassModel.GetAll();

        }

        public ActionResult Edit()
        {
            ViewBag.Club = ClubModel.GetAll();
            ViewBag.IsAthleteUpdate = false;
            ViewBag.IsAthleteDelete = false;
            ViewBag.IsValidInput = true;
            setViewData();
            return View("Edit", new AthleteModel());
        }

        public ActionResult GetAllAthletes(int clubId)
        {
            return Content(new AthleteModel().GetAllByClubId(clubId).ToListboxvalues());            
        }

        public ActionResult GetAthleteDetails(int athleteId)
        {
            return Content(AthleteModel.GetById(athleteId).ToListboxvalues());
        }

        /// <summary>
        /// Creates the specified TXT first name.
        /// </summary>
        /// <param name="txtFirstName">First name.</param>
        /// <param name="txtLastName">Last name.</param>
        /// <param name="txtEmail">The email.</param>
        /// <param name="txtPostalAddress">Postal address.</param>
        /// <param name="txtPostalCode">The postal code.</param>
        /// <param name="txtCity">The city.</param>
        /// <param name="txtPhoneNumber">The phone number.</param>
        /// <param name="genderId">The gender id.</param>
        /// <param name="birthdateId">The birthdate id.</param>
        /// <param name="txtStartNumber">The start number.</param>
        /// <param name="clubId">The club id.</param>
        /// <param name="classId">The class id.</param>
        /// <returns></returns>
        public ActionResult Create(string txtFirstName, string txtLastName, string txtEmail, string txtPostalAddress, string txtPostalCode, string txtCity,
                                    string txtPhoneNumber, string genderId, string birthdateId, string txtStartNumber, string clubId, string classId)
        {
            int startnum = 0;
                int.TryParse(txtStartNumber, out startnum);
            if (!IsValidInput(txtFirstName,txtLastName,txtEmail,txtStartNumber,clubId) ||
                AthleteModel.StartnumberExistsInDb(startnum))
            {
                ViewBag.IsValidInput = false;
                ViewBag.IsAthleteCreated = false;                
                setViewData();
                return View("Index", ClubModel.GetAll());
            }            
            else
            {
                int birthdate = 0;
                int gender = 0, athleteclubid = 0, athleteclassid = 0;

                int.TryParse(genderId, out gender);
                int.TryParse(birthdateId, out birthdate);                
                int.TryParse(clubId, out athleteclubid);
                int.TryParse(classId, out athleteclassid);

                Gender getGender = getGenderNameById(gender);
                string gendername = "";
                if (getGender != null)
                {
                    gendername = getGender.Name;
                }
                BirthDate getBirthday = getBirthDateById(birthdate);
                int birthyear = 0;
                if (getBirthday != null)
                {
                    birthyear = getBirthday.BirthYear;
                }

                ClubModel athclubobj = null;
                if (athleteclubid != 0)
                {
                    athclubobj = new ClubModel(athleteclubid);
                }

                AthleteClassModel athclassobj = null;
                if (athleteclassid != 0)
                {
                    athclassobj = new AthleteClassModel(athleteclassid);
                }

                AthleteModel athlete = new AthleteModel(txtFirstName, txtLastName, txtEmail, txtPostalAddress,
                                    txtPostalCode, txtCity, gendername, birthyear, txtPhoneNumber, startnum, athclubobj, athclassobj);
                athlete.SaveToDb();
                return RedirectToAction("ResetFormField");
            }
        }

        public ActionResult EditAthleteDetails(string txtathleteId, string txtfirstName, string txtlastName, string txtemail, string txtpostalAddress, string txtpostalCode, string txtcity,
                                    string txtphoneNumber, string txtgender, string txtbirthDate, string txtstartNumber, string txtathleteClass)
        {
            if (!IsValidInput(txtfirstName, txtlastName, txtemail, txtstartNumber))
            {
                return Content("invalid");
            }
            else
            {
                int athleteid = 0;
                int.TryParse(txtathleteId, out athleteid);
                int birthdate = 0, startnum = 0;
                int gender = 0, athleteclass = 0;


                int.TryParse(txtgender, out gender);
                int.TryParse(txtbirthDate, out birthdate);
                int.TryParse(txtstartNumber, out startnum);
                int.TryParse(txtathleteClass, out athleteclass);

                Gender getGender = getGenderNameById(gender);
                string gendername = "";
                if (getGender != null)
                {
                    gendername = getGender.Name;
                }
                BirthDate getBirthday = getBirthDateById(birthdate);
                int birthyear = 0;
                if (getBirthday != null)
                {
                    birthyear = getBirthday.BirthYear;
                }

                Athlete athleteDb = new Athlete();
                athleteDb.ID = athleteid;
                athleteDb.FirstName = txtfirstName;
                athleteDb.LastName = txtlastName;
                athleteDb.Email = txtemail;
                athleteDb.PostalAddress = txtpostalAddress;
                athleteDb.PostalCode = txtpostalCode;
                athleteDb.PostalPlace = txtcity;
                athleteDb.Gender = gendername;
                athleteDb.Birthday = birthyear;
                athleteDb.Phone = txtphoneNumber;
                athleteDb.Startnumber = startnum;
                athleteDb.ClassID = athleteclass;

                AthleteModel athlete = new AthleteModel(athleteDb);

                athlete.SaveToDb();
                return Content("updated");
            }
        }

        public ActionResult DeleteAthlete(string ddAthlete)
        public ActionResult Delete(int id, int clubid)
        {
            DeleteAthlete(id.ToString());
            ViewBag.ClubId = clubid;
            return View("Athletes", AthleteModel.GetAthletes(clubid));
        }

        {         
            int Id = 0;
            int.TryParse(ddAthlete, out Id);
            AthleteModel athlete = new AthleteModel(Id);
            athlete.DeleteFromDb();
            return RedirectToAction("ResetDeleteFormField");                  
        }

        public bool IsValidInput(string firstname, string lastname, string email, string startnumber, string clubId = "1")
        {
            int startnum = 0;
            if (String.IsNullOrEmpty(firstname) || String.IsNullOrEmpty(lastname) || String.IsNullOrEmpty(clubId)
                || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(startnumber) || (!isValidEmail(email))
                || (!int.TryParse(startnumber, out startnum)))
            {
                return false;
            }            
            else
            {
                return true;
            }
        }

        public ActionResult ResetFormField()
        {
            setViewData();
            ViewBag.IsAthleteCreated = true;            
            ViewBag.IsValidInput = true;
            return View("Index", ClubModel.GetAll());
        }

        public ActionResult ResetEditFormField()
        {
            setViewData();
            ViewBag.Club = ClubModel.GetAll();
            ViewBag.IsAthleteUpdate = true;
            ViewBag.IsAthleteDelete = false;            
            ViewBag.IsValidInput = true;
            return View("Edit", new AthleteModel());
        }

        public ActionResult EditFormFail()
        {
            setViewData();
            ViewBag.Club = ClubModel.GetAll();
            ViewBag.IsAthleteUpdate = false;
            ViewBag.IsAthleteDelete = false;            
            ViewBag.IsValidInput = false;
            return View("Edit", new AthleteModel());
        }

        public ActionResult ResetDeleteFormField()
        {
            setViewData();
            ViewBag.Club = ClubModel.GetAll();
            ViewBag.IsAthleteDelete = true;
            ViewBag.IsAthleteUpdate = false;
            ViewBag.IsValidInput = true;             
            return View("Edit", new AthleteModel());
        }

        public static bool isValidEmail(string inputEmail)
        {            
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private List<Gender> getGender()
        {
            List<Gender> gender = new List<Gender>()
            {
                new Gender() { Id = 1, Name = "M" },
                new Gender() { Id = 2, Name = "K" }
            };
            gender.Insert(0, null);
            return gender;
        }

        public Gender getGenderNameById(int id)
        {
            List<Gender> gender = new List<Gender>()
            {
                new Gender() { Id = 1, Name = "M" },
                new Gender() { Id = 2, Name = "K" }
            };
            return (gender.SingleOrDefault(gid => gid.Id == id));
        }

        private List<BirthDate> getBirthDate()
        {
            int startdate = 1910;
            int id = 1;
            List<BirthDate> birthdate = new List<BirthDate>();
            while (startdate <= DateTime.Now.Year)
            {
                birthdate.Add(new BirthDate() { Id = id++, BirthYear = startdate++ });
            }
            birthdate.Insert(0, null);
            return birthdate;
        }

        private BirthDate getBirthDateById(int birthid)
        {
            int id = 1;
            int startdate = 1910;
            List<BirthDate> birthdate = new List<BirthDate>();
            while (startdate <= DateTime.Now.Year)
            {
                birthdate.Insert(0, new BirthDate() { Id = id++, BirthYear = startdate++ });
            }
            return birthdate.SingleOrDefault(bday => bday.Id == birthid);
        }

        [HttpGet]
        public ActionResult Athletes(int clubId)
        {
            ViewBag.ClubId = clubId;
            return View("Athletes", AthleteModel.GetAthletes(clubId));
        }


        /// <summary>
        /// Prints the athletes.
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintForClub(int clubId)
        {
            ViewBag.ClubName = ClubModel.GetById(clubId).Name;
            var athletes = AthleteModel.GetAthletes(clubId);
            return View(athletes);
        }

        [HttpGet]
        public ActionResult AthletesForRace(int raceId)
        {
            ViewBag.RaceId = raceId;
            return View("AthletesForRace", AthleteModel.GetAthletesForRace(raceId));
        }


        /// <summary>
        /// Prints the athletes.
        /// </summary>
        /// <returns></returns>
        public ActionResult Print(int raceId)
        {
            ViewBag.RaceName = RaceModel.GetById(raceId).Name;
            var athletes = AthleteModel.GetAthletesForRace(raceId);
            return View(athletes);
        }
    }
}
