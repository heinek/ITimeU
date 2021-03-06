﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ITimeU.Models
{
    public class AthleteClassModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Navn")]
        public string Name { get; set; }

        private static Entities entitiesStatic = new Entities();

        private bool instanceSaved
        {
            get
            {
                if (Id == 0)
                    return false;
                return true;
            }
        }

        public AthleteClassModel(string name)
        {
            Name = name;
            Id = 0;
        }

        public AthleteClassModel(int id)
        {
            this.Id = id;
            var entities = new Entities();
            this.Name = entities.AthleteClasses.Single(athcls => athcls.ID == id).Name;
        }

        public AthleteClassModel(AthleteClass athleteClass)
        {
            Id = athleteClass.ID;
            Name = athleteClass.Name;
        }

        public AthleteClassModel()
        {
            // TODO: Complete member initialization
        }

        internal int SaveToDb()
        {
            var context = new Entities();

            if (!instanceSaved)
                Id = GetOrCreateDbEntity(context);
            else
                updateDbEntity(context);

            return Id;
        }

        private int GetOrCreateDbEntity(Entities context)
        {
            AthleteClassModel clubDb = AthleteClassModel.GetOrCreate(Name);
            return clubDb.Id;
            /*
            AthleteClass athleteClass = new AthleteClass();
            updateProperties(athleteClass);
            context.AthleteClasses.AddObject(athleteClass);
            context.SaveChanges();

            return athleteClass.ID;
            */
        }

        private void updateProperties(AthleteClass athleteClass)
        {
            athleteClass.Name = Name;
        }

        private void updateDbEntity(Entities context)
        {
            AthleteClass athleteClass = context.AthleteClasses.Single(enitity => enitity.ID == Id);
            updateProperties(athleteClass);
            context.SaveChanges();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            AthleteClassModel other = (AthleteClassModel)obj;

            return Id == other.Id && Name.Equals(other.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Retrieves all athlete classes in the database.
        /// </summary>
        /// <returns></returns>
        public static List<AthleteClassModel> GetAll()
        {
            using (var context = new Entities())
            {
                return context.AthleteClasses.Select(clss => new AthleteClassModel()
                {
                    Id = clss.ID,
                    Name = clss.Name
                }).OrderBy(clss => clss.Name).ToList();
            }
            //athleteClassModels.Insert(0, null);
        }

        public static AthleteClassModel GetOrCreate(string name)
        {
            AthleteClass athleteClassDb = null;

            try
            {
                athleteClassDb = getDbEntry(name);
            }
            catch (InvalidOperationException)
            {
                athleteClassDb = createDbEntry(name);
            }

            return new AthleteClassModel(athleteClassDb);
        }

        private static AthleteClass getDbEntry(string name)
        {
            return entitiesStatic.AthleteClasses.Single(temp => temp.Name == name);
        }

        private static AthleteClass createDbEntry(string name)
        {
            AthleteClass athleteClassDb = new AthleteClass();
            athleteClassDb.Name = name;
            saveDbEntry(athleteClassDb);

            return athleteClassDb;
        }

        private static void saveDbEntry(AthleteClass athleteClassDb)
        {
            entitiesStatic.AthleteClasses.AddObject(athleteClassDb);
            entitiesStatic.SaveChanges();
        }

        public static void DeleteIfExists(string name)
        {
            try
            {
                AthleteClass athleteClassDb = entitiesStatic.AthleteClasses.Single(temp => temp.Name == name);
                entitiesStatic.AthleteClasses.DeleteObject(athleteClassDb);
                entitiesStatic.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                // No DB entry found, do noting
            }
        }

        public static AthleteClassModel GetById(int id)
        {
            using (var context = new Entities())
            {
                var model = context.AthleteClasses.Where(athleteclass => athleteclass.ID == id).Single();
                return new AthleteClassModel()
                {
                    Id = model.ID,
                    Name = model.Name
                };
            }
        }

        public void Update()
        {
            using (var context = new Entities())
            {
                var model = context.AthleteClasses.Where(athleteclass => athleteclass.ID == Id).Single();
                model.Name = Name;
                context.SaveChanges();
            }
        }

        public void Delete()
        {
            using (var context = new Entities())
            {
                var model = context.AthleteClasses.Where(athleteclass => athleteclass.ID == Id).Single();
                context.DeleteObject(model);
                context.SaveChanges();
            }
        }
    }
}