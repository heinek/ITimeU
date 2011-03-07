using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class ClubModel
    {
        private Club club;

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

        public ClubModel(string name)
        {
            this.Name = name;
            Id = 0;
        }

        public ClubModel(Club club)
        {
            Id = club.ClubID;
            Name = club.Name;
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
            Club club = new Club();
            updateProperties(club);
            context.Clubs.AddObject(club);
            context.SaveChanges();

            return club.ClubID;
        }

        private void updateProperties(Club club)
        {
            club.Name = Name;
        }

        private void updateDbEntity(Entities context)
        {
            Club club = context.Clubs.Single(enitity => enitity.ClubID == Id);
            updateProperties(club);
            context.SaveChanges();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ClubModel other = (ClubModel)obj;

            return Id == other.Id && Name.Equals(other.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return "[Club, id: " + Id + ", name: " + Name + "]";
        }
    }
}