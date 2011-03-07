using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class AthleteClassModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private bool dbEntryCreated
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

        public AthleteClassModel(AthleteClass athleteClass)
        {
            Id = athleteClass.ID;
            Name = athleteClass.Name;
        }

        internal int SaveToDb()
        {
            var context = new Entities();

            if (!dbEntryCreated)
                Id = CreateDbEntity(context);
            else
                updateDbEntity(context);

            return Id;
        }

        private int CreateDbEntity(Entities context)
        {
            AthleteClass athleteClass = new AthleteClass();
            updateProperties(athleteClass);
            context.AthleteClasses.AddObject(athleteClass);
            context.SaveChanges();

            return athleteClass.ID;
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
            return "[Athlete Class, id: " + Id + ", name: " + Name + "]";
        }
    }
}