
namespace ITimeU.Models
{
    public class DBAdminModel
    {
        public static bool DeleteAll()
        {
            using (var context = new Entities())
            {
                try
                {

                    var raceintermediates = context.RaceIntermediates;
                    foreach (var raceintermediate in raceintermediates)
                    {
                        context.DeleteObject(raceintermediate);
                    }
                    var runtimes = context.Runtimes;
                    foreach (var runtime in runtimes)
                    {
                        context.DeleteObject(runtime);
                    }
                    var raceathletes = context.RaceAthletes;
                    foreach (var raceathlete in raceathletes)
                    {
                        context.DeleteObject(raceathlete);
                    }
                    var athletes = context.Athletes;
                    foreach (var athlete in athletes)
                    {
                        context.DeleteObject(athlete);
                    }
                    var classes = context.AthleteClasses;
                    foreach (var athleteClass in classes)
                    {
                        context.DeleteObject(athleteClass);
                    }
                    var orders = context.CheckpointOrders;
                    foreach (var order in orders)
                    {
                        context.DeleteObject(order);
                    }
                    var checkpoints = context.Checkpoints;
                    foreach (var checkpoint in checkpoints)
                    {
                        context.DeleteObject(checkpoint);
                    }
                    var clubs = context.Clubs;
                    foreach (var club in clubs)
                    {
                        context.DeleteObject(club);
                    }
                    var timers = context.Timers;
                    foreach (var time in timers)
                    {
                        context.DeleteObject(time);
                    }
                    var races = context.Races;
                    foreach (var race in races)
                    {
                        context.DeleteObject(race);
                    }
                    var evnts = context.Events;
                    foreach (var evnt in evnts)
                    {
                        context.DeleteObject(evnt);
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }
}