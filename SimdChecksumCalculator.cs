using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

public static class SimdChecksumCalculator
{
    private const int MaxDegreeOfParallelism = 64;
    private const int ChunkSize = 1_048_576;

    public static long ComputeChecksumSimd(ReadOnlySpan<byte> data)
    {
        long totalSum = 0;
        int i = 0;

        // Check if the CPU supports hardware SIMD acceleration and data is large enough
        if (Vector.IsHardwareAccelerated && data.Length >= Vector<byte>.Count)
        {
            // Cast the byte span directly to a Vector<byte> span with zero allocations
            ReadOnlySpan<Vector<byte>> vectors = MemoryMarshal.Cast<byte, Vector<byte>>(data);
            
            Vector<uint> accumulator = Vector<uint>.Zero;

            foreach (ref readonly Vector<byte> vec in vectors)
            {
                // Widen 32 bytes -> two 16-element ushort vectors to prevent byte overflow
                Vector.Widen(vec, out Vector<ushort> lowShorts, out Vector<ushort> highShorts);

                // Widen ushorts -> four 8-element uint vectors and accumulate
                Vector.Widen(lowShorts, out Vector<uint> v1, out Vector<uint> v2);
                Vector.Widen(highShorts, out Vector<uint> v3, out Vector<uint> v4);

                accumulator += v1 + v2 + v3 + v4;
            }

            // Sum the 8 parallel lanes inside the final SIMD register
            for (int lane = 0; lane < Vector<uint>.Count; lane++)
            {
                totalSum += accumulator[lane];
            }

            // Advance index past all bytes processed by SIMD vectors
            i = vectors.Length * Vector<byte>.Count;
        }

        // Process any remaining leftover bytes (tail loop)
        for (; i < data.Length; i++)
        {
            totalSum += data[i];
        }

        return totalSum;
    }

    public static long ComputeChecksumParallel(ReadOnlyMemory<byte> data)
    {
        long totalSum = 0;
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = MaxDegreeOfParallelism
        };

        Parallel.ForEach(
            Partitioner.Create(0, data.Length, ChunkSize),
            options,
            () => 0L,
            (range, _, localSum) =>
                localSum + ComputeChecksumSimd(data.Span.Slice(range.Item1, range.Item2 - range.Item1)),
            localSum => Interlocked.Add(ref totalSum, localSum));

        return totalSum;
    }
}
