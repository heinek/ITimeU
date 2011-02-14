using System;

namespace ITimeU.Models
{
    [Serializable]
    public class ParticipantModel
    {
        public int Id { get; set; }

        private string firstName;
        private string surName;
        private string postalAddress;
        private string postalCode;
        private string postalPlace;
        private string phone;
        private string eMail;
        private int club;
        private DateTime birthday;
        private string gender;
        private Boolean isDeleted;

        public string FirstName
        {
            get 
            { 
                return firstName;
            }
            private set 
            { 
                firstName = value; 
            }
        }

        public string SurName
        {
            get
            {
                return surName;
            }
            private set
            {
                surName = value;
            }
        }

        public string PostalAddress
        {
            get
            {
                return postalAddress;
            }
            private set
            {
                postalAddress = value;
            }
        }

        public string PostalCode
        {
            get
            {
                return postalCode;
            }
            private set
            {
                postalCode = value;
            }
        }

        public string PostalPlace
        {
            get
            {
                return postalPlace;
            }
            private set
            {
                postalPlace = value;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
            private set
            {
                isDeleted = value;
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }
            private set
            {
                phone = value;
            }
        }

        public string EMail
        {
            get
            {
                return eMail;
            }
            private set
            {
                eMail = value;
            }
        }

        public int Club
        {
            get
            {
                return club;
            }
            private set
            {
                club = value;
            }
        }

        public DateTime Birthday
        {
            get
            {
                return birthday;
            }
            private set
            {
                birthday = value;
            }
        }

        public string Gender
        {
            get
            {
                return gender;
            }
            private set
            {
                gender = value;
            }
        }

        


        public ParticipantModel()
        {

        }

    }
}
