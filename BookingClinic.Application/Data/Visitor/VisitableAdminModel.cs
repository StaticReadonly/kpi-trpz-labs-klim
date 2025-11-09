using BookingClinic.Application.Interfaces.Visitor;

namespace BookingClinic.Application.Data.Visitor
{
    public class VisitableAdminModel : VisitableModelBase
    {
        public VisitableAdminModel(
            string name,
            string surname)
                : base(name, surname)
        {
        }

        public override void AcceptVisitor(IUserVisitor visitor)
        {
            visitor.VisitAdmin(this);
        }
    }
}
