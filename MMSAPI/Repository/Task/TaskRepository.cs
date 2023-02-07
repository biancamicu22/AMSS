using DataLibrary;
using DataLibrary.DTO;
using DataLibrary.Models;
using DataLibrary.SMTPServer;
using DataLibrary.StatePattern;
using MimeKit;
using System;
using System.Linq;
using System.Net.Mail;

namespace MMSAPI.Repository
{
    public class TaskRepository : BaseRepository<AppTask>, ITaskRepository
    {
        public SMTP smtp = new SMTP();
        public TaskRepository(MMSContext context) : base(context)
        {

        }
        public void AssignTaskEmail(AppTask task)
        {
            try
            {
                var message = smtp.ToDoMessage(task);
                this.smtp.smtpClient.Send(message);
            }
            catch (Exception e)
            {

            }
        }

        public void CompleteTask(string taskID)
        {
            var smtp = new SMTP();
            try
            {
                var assignedTask = _context.Set<AppTask>().ToList();
                foreach (var x in assignedTask)
                {
                    if (x.Id.ToString() == taskID)
                    {
                        x.State = new CompletedState();
                        x.State.Change(x);
                        var message = smtp.DoneMessage(x);
                        this.smtp.smtpClient.Send(message);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
        public void AssignTask(string taskID)
        {
            var smtp = new SMTP();
            try
            {
                var assignedTask = _context.Set<AppTask>().ToList();
                foreach (var x in assignedTask)
                {
                    if (x.Id.ToString() == taskID)
                    {
                        x.State = new AssignedState();
                        x.State.Change(x);
                        var message = smtp.ToDoMessage(x);
                        this.smtp.smtpClient.Send(message);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public void StatTask(string taskID)
        {
            try
            {
                var assignedTask = _context.Tasks.Where(x => x.Id == taskID).FirstOrDefault();
                assignedTask.State = new StartedState();
                assignedTask.State.Change(assignedTask);
                var message = smtp.InProgressMessage(assignedTask);
                this.smtp.smtpClient.Send(message);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}
