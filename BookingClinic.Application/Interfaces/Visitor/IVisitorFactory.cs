namespace BookingClinic.Application.Interfaces.Visitor
{
    public interface IVisitorFactory
    {
        IUserVisitor CreatePDFVisitor(string filename);
    }
}
