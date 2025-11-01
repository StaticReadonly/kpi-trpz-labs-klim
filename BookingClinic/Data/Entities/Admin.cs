using BookingClinic.Services.Visitor;

namespace BookingClinic.Data.Entities
{
    public class Admin : UserBase
    {
        public override void AcceptVisitor(IUserVisitor visitor)
        {
            visitor.VisitAdmin(this);
        }
    }
}
