namespace BookingClinic.Application.Data.User
{
    public class UserPictureDto : IDisposable
    {
        public UserPictureDto(string fileName, Stream fileStream)
        {
            this.FileName = fileName;
            this.FileStream = fileStream;
        }

        public string FileName { get; set; }
        public Stream? FileStream { get; set; }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                FileStream?.Dispose();
            }

            FileStream = null;
            _disposed = true;
        }
    }
}
