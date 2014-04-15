using GraceChurchKelseyvilleAwana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class AttendanceController : Controller
    {
        private const int NUMBER_OF_WEEKS_TO_SHOW = 3;
        private const int DAYS_IN_WEEK = 7;
        private DateTime _lastAwanaDate;
        private DateTime _lastDateToShow;
        private Entities db = new Entities();

        //
        // GET: /Attendance/
        public ActionResult Index()
        {
            _lastAwanaDate = LastAwanaDate();
            _lastDateToShow = _lastAwanaDate.AddDays(- (DAYS_IN_WEEK * (NUMBER_OF_WEEKS_TO_SHOW - 1)));

            var attendances = db.Attendances.ToList();
            var students = StudentsUserHasAccessTo(null);
            if(GenerateAttendancesIfNeeded(_lastDateToShow, _lastAwanaDate, 
                students, attendances))
            {
                attendances = db.Attendances.ToList();
            }

            return View(students);
        }

        public DateTime LastAwanaDate()
        {
            var lastAwanaDate = DateTime.Today;

            while (!lastAwanaDate.DayOfWeek.Equals(Constants.DayOfAwana))
            {
                lastAwanaDate = lastAwanaDate.AddDays(-1.0);
            }

            return lastAwanaDate;
        }

        //Return value indicates whether any attendances were added
        public bool GenerateAttendancesIfNeeded(DateTime startDate, DateTime finishDate, 
            List<Student> students, List<Attendance> existingAttendances)
        {
            var currentDate = startDate;
            var attendancesAdded = false;

            while (currentDate <= finishDate)
            {
                foreach (var student in students)
                {
                    if (!(existingAttendances.Exists(x => x.StudentID == student.StudentID 
                        && x.AttendanceDate.Equals(currentDate))))
                    {
                        attendancesAdded = true;
                        db.Attendances.Add(new Attendance
                        {
                            StudentID = student.StudentID,
                            AttendanceDate = currentDate,
                            Attended = false
                        });
                    }
                }
                currentDate = currentDate.AddDays(DAYS_IN_WEEK);
            }
            if (attendancesAdded)
            {
                db.SaveChanges();
            }
            return attendancesAdded;
        }

        // Unimplimented for now
        public List<Student> StudentsUserHasAccessTo(AspNetUser user)
        {
            return db.Students.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}