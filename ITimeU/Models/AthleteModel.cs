using System;

namespace ITimeU.Models
{
    [Serializable]
    public class AthleteModel
    {
        public int Id { get; private set; }
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

        public static AthleteModel GetById(int id)
        {
            return null;
        }

        public AthleteModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = 0;
        }

        public void SaveToDb()
        {
            var context = new Entities();

            if (!dbEntryCreated)
                Id = CreateDbEntity(context);
            else
                updateDbEntry(context);
        }


    }
}
