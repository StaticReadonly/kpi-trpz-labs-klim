using BookingClinic.Services.Visitor;

namespace BookingClinic.Data.Entities
{
    public class Patient : UserBase
    {
        public override void AcceptVisitor(IUserVisitor visitor)
        {
            visitor.VisitPatient(this);
        }
    }
}
