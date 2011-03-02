﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace ITimeU.Models
{
    [Serializable]
    public class AthleteModel
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        private bool dbEntryCreated
        {
            get
            {
                if (Id == 0)
                    return false;
                else
                    return true;
            }
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PostalAddress { get; private set; }
        public string PostalCode { get; private set; }
        public string PostalPlace { get; private set; }
        public string Phone { get; private set; }
        public string EMail { get; private set; }
        public int Club { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Gender { get; private set; }
        public static AthleteModel GetById(int idToGet)
        {
            var entities = new Entities();
            Athlete athleteDb = entities.Athletes.Single(temp => temp.ID == idToGet);
        
            return new AthleteModel(athleteDb);
        }

        /// <summary>
        /// Retrieves all athletes in the database.
        /// </summary>
        /// <returns></returns>
        public static List<AthleteModel> GetAll()
        {
            var entities = new Entities();
            IEnumerable<Athlete> athletes = entities.Athletes.AsEnumerable<Athlete>();

            List<AthleteModel> athleteModels = new List<AthleteModel>();
            foreach (Athlete athleteDb in athletes)
            {
                AthleteModel athleteModel = new AthleteModel(athleteDb);
                athleteModels.Add(athleteModel);
            }

            return athleteModels;
        }
        
        public AthleteModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = 0;
        }

        public AthleteModel(Athlete athleteDb)
        {
            Id = athleteDb.ID;
            FirstName = athleteDb.FirstName;
            LastName = athleteDb.LastName;
        }

        /// <summary>
        /// Saves a list of athletes to the database.
        /// </summary>
        /// <param name="athletes">The list of athletes to save.</param>
        public static void SaveToDb(List<AthleteModel> athletes)
        {
            Entities context = new Entities();
            foreach (AthleteModel athlete in athletes)
            {
                Athlete athleteDb = createAthleteDbFrom(athlete);
                context.Athletes.AddObject(athleteDb);
            }
            context.SaveChanges();
        }

        private static Athlete createAthleteDbFrom(AthleteModel athlete)
        {
            Athlete athleteDb = new Athlete();
            athleteDb.FirstName = athlete.FirstName;
            athleteDb.LastName = athleteDb.LastName;
            return athleteDb;
        }

        public void SaveToDb()
        {
            var context = new Entities();

            if (!dbEntryCreated)
                Id = CreateDbEntity(context);
            else
                updateDbEntity(context);
        }

        private int CreateDbEntity(Entities context)
        {
            Athlete athlete = new Athlete();
            updateProperties(athlete);
            context.Athletes.AddObject(athlete);
            context.SaveChanges();

            return athlete.ID;
        }

        private void updateProperties(Athlete athlete)
        {
            athlete.FirstName = FirstName;
            athlete.LastName = LastName;
        }

        private int CreateDbEntity(Entities context)
        {
            Athlete athlete = new Athlete();
            updateProperties(athlete);
            context.Athletes.AddObject(athlete);
            context.SaveChanges();

            return athlete.ID;
        }

        private void updateProperties(Athlete athlete)
        {
            athlete.FirstName = FirstName;
            athlete.LastName = LastName;
        }

        private void updateDbEntity(Entities context)
        {
            Athlete athlete = context.Athletes.Single(enitity => enitity.ID == Id);
            updateProperties(athlete);
            context.SaveChanges();
        }

        public override string ToString()
        {
            return FirstName + " " + LastName ;
        }
    }
}
