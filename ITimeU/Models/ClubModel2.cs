using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class ClubModel2 : Model<Club>
    {
        public string Name { get; private set; }

        public ClubModel2() {
            this.Name = null;
        }

        public ClubModel2(string name)
        {
            this.Name = name;
        }

        public override void Instantiate(Club club)
        {
            Id = club.ClubID;
            Name = club.Name;
        }

        public static List<ClubModel2> GetAll()
        {
            return Model<Club>.GetAll<ClubModel2>(new Entities().Clubs);
        }

    }
}