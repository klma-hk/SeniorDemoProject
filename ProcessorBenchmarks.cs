using BenchmarkDotNet.Attributes;
using System;

namespace SeniorCsharpDemo
{
    [MemoryDiagnoser]
    public class ProcessorBenchmarks
    {
        private HighPerformanceBufferProcessor _highPerformanceProcessor = null!;
        private RegularPerformanceProcessor _regularProcessor = null!;
        private DirectBufferProcessor _directBufferProcessor = null!;
        private byte[] _data = null!;

        [Params(10_000, 100_000)] // 10 KB and 100 KB (Large Object Heap)
        public int DataSize { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _highPerformanceProcessor = new HighPerformanceBufferProcessor();
            _regularProcessor = new RegularPerformanceProcessor();
            _directBufferProcessor = new DirectBufferProcessor();
            _data = new byte[DataSize];
            Random.Shared.NextBytes(_data);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _highPerformanceProcessor.Dispose();
            _regularProcessor.Dispose();
            _directBufferProcessor.Dispose();
        }

        [Benchmark]
        public Task<long> HighPerformanceProcessorProcess()
        {
            return _highPerformanceProcessor.ProcessDataAsync(_data, CancellationToken.None);
        }

        [Benchmark(Baseline = true)]
        public Task<long> RegularProcessorProcess()
        {
            return _regularProcessor.ProcessDataAsync(_data, CancellationToken.None);
        }

        [Benchmark]
        public Task<long> DirectBufferProcessorProcess()
        {
            return _directBufferProcessor.ProcessDataAsync(_data, CancellationToken.None);
        }

        [Benchmark]
        public long SimdChecksum()
        {
            return SimdChecksumCalculator.ComputeChecksumSimd(_data);
        }

        [MemoryDiagnoser]
        public class ChecksumBenchmarks
        {
            private byte[] _data = null!;

            [Params(1_000_000, 100_000_000)]
            public int DataSize { get; set; }

            [GlobalSetup]
            public void Setup()
            {
                _data = new byte[DataSize];
                Random.Shared.NextBytes(_data);

                long sequential = SimdChecksumCalculator.ComputeChecksumSimd(_data);
                long parallel = SimdChecksumCalculator.ComputeChecksumParallel(_data);
                if (sequential != parallel)
                {
                    throw new InvalidOperationException("Sequential and parallel checksums do not match.");
                }
            }

            [Benchmark(Baseline = true)]
            public long SimdChecksum()
            {
                return SimdChecksumCalculator.ComputeChecksumSimd(_data);
            }

            [Benchmark]
            public long ParallelSimdChecksum()
            {
                return SimdChecksumCalculator.ComputeChecksumParallel(_data);
            }
        }
    }
}
