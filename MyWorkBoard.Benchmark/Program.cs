// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using MyWorkBoard.Benchmark;

Console.WriteLine("Testy wydjanościowe");

BenchmarkRunner.Run<TrackingBenchmark>();