using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ITimeU.Models
{
    public class EventModel
    {
        public int EventId { get; set; }
        [Required]
        [DisplayName("Navn")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Dato")]
        public DateTime EventDate { get; set; }

        public EventModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="date">The date.</param>
        public EventModel(string name, DateTime date)
        {
            Name = name;
            EventDate = date;
        }

        /// <summary>
        /// Saves this event.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            using (var context = new Entities())
            {
                var newEvent = new Event();
                newEvent.Name = Name;
                newEvent.EventDate = EventDate;
                try
                {
                    context.Events.AddObject(newEvent);
                    context.SaveChanges();
                    EventId = newEvent.EventId;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes this event.
        /// </summary>
        public void Delete()
        {
            using (var context = new Entities())
            {
                var entityToDelete = context.Events.Where(evnt => evnt.EventId == EventId).Single();
                entityToDelete.IsDeleted = true;
                context.SaveChanges();
            }
        }

        public static List<EventModel> GetEvents()
        {
            using (var context = new Entities())
            {
                return context.Events.Where(evnt => evnt.IsDeleted == false).Select(evnt => new EventModel()
                {
                    EventId = evnt.EventId,
                    EventDate = evnt.EventDate,
                    Name = evnt.Name
                }).OrderBy(evnt => evnt.Name).ToList();
            }
        }

        public static EventModel GetById(int id)
        {
            using (var context = new Entities())
            {
                var eventDb = context.Events.Single(evnt => evnt.EventId == id);
                return new EventModel()
                {
                    EventId = eventDb.EventId,
                    Name = eventDb.Name,
                    EventDate = eventDb.EventDate
                };
            }
        }
    }
}