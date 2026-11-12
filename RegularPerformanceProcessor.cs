namespace SeniorCsharpDemo
{
    public class RegularPerformanceProcessor : IDisposable
    {
        private bool _disposed;

         public async Task<long> ProcessDataAsync(byte[] input, CancellationToken cancellationToken)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);

            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();

            long checksum = 0;
            foreach (var b in input)
            {
                checksum += b;
            }
            return checksum;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            GC.SuppressFinalize(this);
        }


    }
}
