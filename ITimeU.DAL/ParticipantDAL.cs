
using System;
using System.Linq;
namespace ITimeU.DAL
{
    public class ParticipantDAL
    {
        public int ParticipantID { get; set; }

        public string FirstName { get; set; }

        public ParticipantDAL()
        {

        }

        public static ParticipantDAL GetParticipantById(int id)
        {
            using (var ctx = new ITimeUEntities())
            {
                var participant = ctx.Participants.Single(prtcpnt => prtcpnt.ParticipantID == id);
                var participantDal = new ParticipantDAL()
                    {
                        ParticipantID = participant.ParticipantID, 
                        FirstName = participant.FirstName
                    };
                return participantDal;
            }
        }

        public static ParticipantDAL Create()
        {
            ParticipantDAL participantDal = new ParticipantDAL();
            using (var ctx = new ITimeUEntities())
            {
                Participant participant = new Participant();
                ctx.Participants.AddObject(participant);
                ctx.SaveChanges();
                participantDal.ParticipantID = ctx.Participants.OrderByDescending(prtcpnt => prtcpnt.ParticipantID).First().ParticipantID;
            }
            return participantDal;
        }

        public void Save()
        {
            using (var ctx = new ITimeUEntities())
            {
                Participant participant = ctx.Participants.Single(prtcpnt => prtcpnt.ParticipantID == ParticipantID);
                participant.FirstName = this.FirstName;
                ctx.SaveChanges();
            }
        }
    }
}
