namespace BookingClinic.Services.Visitor
{
    public interface IVisitorFactory
    {
        IUserVisitor CreatePDFVisitor(string filename);
    }
}
