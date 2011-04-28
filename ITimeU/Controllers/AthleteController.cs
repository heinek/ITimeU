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

        public ActionResult GetAllAthletes(int Id)
        {
            return Content(new AthleteModel().GetAllByClubId(Id).ToListboxvalues());
        }

        public ActionResult GetAthlete(int Id)
        {
            return Content(AthleteModel.GetById(Id).ToListboxvalues());
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
            if (!IsValidInput(txtFirstName, txtLastName, txtEmail, txtStartNumber, clubId))
            {
                ViewBag.IsValidInput = false;
                ViewBag.IsAthleteCreated = false;
                setViewData();
                return View("Index", ClubModel.GetAll());
            }
            else
            {
                int birthdate = 0, startnum = 0;
                int gender = 0, athleteclubid = 0, athleteclassid = 0;

                int.TryParse(genderId, out gender);
                int.TryParse(birthdateId, out birthdate);
                int.TryParse(txtStartNumber, out startnum);
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

        public ActionResult EditAthlete(string btnSubmit, string ddAthlete, string txtFirstName, string txtLastName, string txtEmail, string txtPostalAddress, string txtPostalCode, string txtCity,
                                    string txtPhoneNumber, string ddGender, string ddBirthDate, string txtStartNumber, string ddClass)
        {
            if (btnSubmit.Equals("Delete"))
            {
                DeleteAthlete(ddAthlete);
                return RedirectToAction("ResetDeleteFormField");
            }
            else
            {
                if (!IsValidInput(txtFirstName, txtLastName, txtEmail, txtStartNumber))
                {
                    return RedirectToAction("EditFormFail");
                }
                else
                {
                    int athleteid = 0;
                    int.TryParse(ddAthlete, out athleteid);
                    int birthdate = 0, startnum = 0;
                    int gender = 0, athleteclass = 0;


                    int.TryParse(ddGender, out gender);
                    int.TryParse(ddBirthDate, out birthdate);
                    int.TryParse(txtStartNumber, out startnum);
                    int.TryParse(ddClass, out athleteclass);

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
                    athleteDb.FirstName = txtFirstName;
                    athleteDb.LastName = txtLastName;
                    athleteDb.Email = txtEmail;
                    athleteDb.PostalAddress = txtPostalAddress;
                    athleteDb.PostalCode = txtPostalCode;
                    athleteDb.PostalPlace = txtCity;
                    athleteDb.Gender = gendername;
                    athleteDb.Birthday = birthyear;
                    athleteDb.Phone = txtPhoneNumber;
                    athleteDb.Startnumber = startnum;
                    athleteDb.ClassID = athleteclass;

                    AthleteModel athlete = new AthleteModel(athleteDb);

                    athlete.SaveToDb();
                    return RedirectToAction("ResetEditFormField");
                }
            }

        }

        public ActionResult Delete(int id, int clubid)
        {
            DeleteAthlete(id.ToString());
            ViewBag.ClubId = clubid;
            return View("Athletes", AthleteModel.GetAthletes(clubid));
        }

        private void DeleteAthlete(string id)
        {
            int Id = 0;
            int.TryParse(id, out Id);
            AthleteModel athlete = new AthleteModel(Id);
            athlete.DeleteFromDb();
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
            else if (AthleteModel.StartnumberExistsInDb(startnum))
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
            //ViewBag.IsValidEmail = true;
            ViewBag.IsValidInput = true;
            return View("Index", ClubModel.GetAll());
        }

        public ActionResult ResetEditFormField()
        {
            setViewData();
            ViewBag.Club = ClubModel.GetAll();
            ViewBag.IsAthleteUpdate = true;
            ViewBag.IsAthleteDelete = false;
            //ViewBag.IsValidEmail = true;
            ViewBag.IsValidInput = true;
            return View("Edit", new AthleteModel());
        }

        public ActionResult EditFormFail()
        {
            setViewData();
            ViewBag.Club = ClubModel.GetAll();
            ViewBag.IsAthleteUpdate = false;
            ViewBag.IsAthleteDelete = false;
            //ViewBag.IsValidEmail = true;
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
            //inputEmail = NulltoString(inputEmail);
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
