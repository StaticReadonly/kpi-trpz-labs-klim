using BookingClinic.Application.Interfaces.Visitor;

namespace BookingClinic.Application.Data.Visitor
{
    public abstract class VisitableModelBase
    {
        protected VisitableModelBase(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public string Name { get; set; }
        public string Surname { get; set; }

        public abstract void AcceptVisitor(IUserVisitor visitor);
    }
}
