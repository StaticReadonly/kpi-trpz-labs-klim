namespace BookingClinic.Services.Visitor
{
    public class VisitorFactory : IVisitorFactory
    {
        public IUserVisitor CreatePDFVisitor(string filename)
        {
            return new UserVisitor(filename);
        }
    }
}
