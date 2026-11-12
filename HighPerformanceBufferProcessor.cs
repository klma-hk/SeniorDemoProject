using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorCsharpDemo
{
    public sealed class HighPerformanceBufferProcessor : IDisposable
    {
        private readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;
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

                byte[] rentedArray = _pool.Rent(input.Length);
                try
                {
                    Span<byte> destination = rentedArray.AsSpan(0, input.Length);
                    input.Span.CopyTo(destination);

                    long checksum = 0;
                    foreach (ref readonly byte b in destination)
                    {
                        checksum += b;
                    }
                    return checksum;
                }
                finally
                {
                    _pool.Return(rentedArray);
                }
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
