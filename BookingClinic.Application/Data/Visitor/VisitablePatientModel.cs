using BookingClinic.Application.Interfaces.Visitor;

namespace BookingClinic.Application.Data.Visitor
{
    public class VisitablePatientModel : VisitableModelBase
    {
        public VisitablePatientModel(
            string name,
            string surname,
            string email, 
            string phone, 
            int appointmetsCount, 
            int finishedAppointmentsCount, 
            int canceledAppointmentsCount)
                : base(name, surname)
        {
            Email = email;
            Phone = phone;
            AppointmetsCount = appointmetsCount;
            FinishedAppointmentsCount = finishedAppointmentsCount;
            CanceledAppointmentsCount = canceledAppointmentsCount;
        }

        public string Email { get; set; }
        public string Phone { get; set; }
        public int AppointmetsCount { get; set; }
        public int FinishedAppointmentsCount { get; set; }
        public int CanceledAppointmentsCount { get; set; }

        public override void AcceptVisitor(IUserVisitor visitor)
        {
            visitor.VisitPatient(this);
        }
    }
}
