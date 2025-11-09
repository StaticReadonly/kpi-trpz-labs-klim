using BookingClinic.Application.Interfaces.Visitor;

namespace BookingClinic.Application.Data.Visitor
{
    public class VisitableDoctorModel : VisitableModelBase
    {
        public VisitableDoctorModel(
            string name,
            string surname,
            string speciality,
            string clinic,
            string email,
            int appointmetsCount,
            int finishedAppointmentsCount,
            int canceledAppointmentsCount)
            : base(name, surname)
        {
            Speciality = speciality;
            Clinic = clinic;
            Email = email;
            AppointmetsCount = appointmetsCount;
            FinishedAppointmentsCount = finishedAppointmentsCount;
            CanceledAppointmentsCount = canceledAppointmentsCount;
        }

        public string Speciality { get; set; }
        public string Clinic { get; set; }
        public string Email { get; set; }
        public int AppointmetsCount { get; set; }
        public int FinishedAppointmentsCount { get; set; }
        public int CanceledAppointmentsCount { get; set; }

        public override void AcceptVisitor(IUserVisitor visitor)
        {
            visitor.VisitDoctor(this);
        }
    }
}
