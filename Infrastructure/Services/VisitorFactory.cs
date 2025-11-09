using BookingClinic.Application.Interfaces.Visitor;

namespace BookingClinic.Infrastructure.Services
{
    public class VisitorFactory : IVisitorFactory
    {
        public IUserVisitor CreatePDFVisitor(string filename)
        {
            return new UserVisitor(filename);
        }
    }
}
