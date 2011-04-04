using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Models;
using System.Text.RegularExpressions;

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

        private void setViewData()
        {            
            ViewBag.Gender = getGender();
            ViewBag.BirthDate = getBirthDate();
            ViewBag.AthleteClass = AthleteClassModel.GetAll();                       
        }

        public ActionResult Create(string txtFirstName, string txtLastName, string txtEmail, string txtPostalAddress, string txtPostalCode, string txtCity,
                                    string txtPhoneNumber, string genderId, string birthdateId, string txtStartNumber, string clubId, string classId )
        {
            if (!IsValidInput(txtStartNumber,txtLastName,txtEmail,txtStartNumber))
            {                
                ViewBag.IsValidInput = false;
                ViewBag.IsAthleteCreated = false;
                //ViewBag.IsValidEmail = false;
                setViewData();
                return View("Index", ClubModel.GetAll());
            }
            else 
            {
                int birthdate = 0, startnum = 0;
                int gender = 0, athleteclub = 0, athleteclass = 0;

                int.TryParse(genderId, out gender);
                int.TryParse(birthdateId, out birthdate);
                int.TryParse(txtStartNumber, out startnum);
                int.TryParse(clubId, out athleteclub);
                int.TryParse(classId, out athleteclass);

                Gender g = getGenderNameById(gender);
                string gendername = g.Name;
                int birthyear = getBirthDateById(birthdate).BirthYear;

                ClubModel athclub = new ClubModel(athleteclub);
                AthleteClassModel athclass = new AthleteClassModel(athleteclass);

                AthleteModel athlete = new AthleteModel(txtFirstName, txtLastName, txtEmail, txtPostalAddress,
                                    txtPostalCode, txtCity, gendername, birthyear, txtPhoneNumber, startnum, athclub, athclass);
                athlete.SaveToDb();        
                return RedirectToAction("ResetFormField");
            }            
        }

        public bool IsValidInput(string firstname, string lastname, string email, string startnumber)
        {
            int startnum = 0;
            if (String.IsNullOrEmpty(firstname) || String.IsNullOrEmpty(lastname)
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
            //ViewBag.IsValidEmail = true;
            ViewBag.IsValidInput = true;
            return View("Index", ClubModel.GetAll());
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
                new Gender() { Id = 2, Name = "F" }
            };
            return gender;
        }

        public Gender getGenderNameById(int id)
        {
            List<Gender> gender = new List<Gender>()
            {
                new Gender() { Id = 1, Name = "M" },
                new Gender() { Id = 2, Name = "F" }
            };
            return (gender.Single(gid => gid.Id == id));
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
            return birthdate;
        }

        private BirthDate getBirthDateById(int birthid)
        {
            int id = 1;
            int startdate = 1910;
            List<BirthDate> birthdate = new List<BirthDate>();
            while (startdate <= DateTime.Now.Year)
            {
                birthdate.Add(new BirthDate() { Id = id++, BirthYear = startdate++ });
            }
            return birthdate.Single(bday => bday.Id == birthid);
        }
    }
}
