using System;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorCsharpDemo
{
    public sealed class DirectBufferProcessor : IDisposable
    {
        private bool _disposed;
        private int _activeOperations;

        public async Task<long> ProcessDataAsync(ReadOnlyMemory<byte> input, CancellationToken cancellationToken)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);

            Interlocked.Increment(ref _activeOperations);
            try
            {
                await Task.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                long checksum = 0;
                foreach (ref readonly byte b in input.Span)
                {
                    checksum += b;
                }
                return checksum;
            }
            finally
            {
                Interlocked.Decrement(ref _activeOperations);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            while (Volatile.Read(ref _activeOperations) > 0)
            {
                Thread.SpinWait(10);
            }
            GC.SuppressFinalize(this);
        }
    }
}
