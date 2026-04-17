using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace NLanguageTag.Benchmark
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkRunner.Run<Parse>();
		}
	}

	[RPlotExporter]
	[HtmlExporter]
	[MinColumn, MaxColumn, MeanColumn, MedianColumn]
	[MemoryDiagnoser]
	public class Parse
	{
		private int _languageIndex = 0;
		private int _regionIndex = 0;
		private int _scriptIndex = 0;
		private string _textL = string.Empty;
		private string _textLSR = string.Empty;
		private string _textLSRE = string.Empty;

		[GlobalSetup]
		public void GlobalSetup()
		{
		}

		[IterationSetup]
		public void IterationSetup()
		{
			_textL = SubtagSamples.Languages[_languageIndex];

			_textLSR = string.Join(
				"-",
				SubtagSamples.Languages[_languageIndex],
				SubtagSamples.Scripts[_scriptIndex],
				SubtagSamples.Regions[_regionIndex]);

			_textLSRE = _textLSR + "-a-aaa-bbb";

			_languageIndex = (_languageIndex + 17) % SubtagSamples.Languages.Count;
			_regionIndex = (_regionIndex + 19) % SubtagSamples.Regions.Count;
			_scriptIndex = (_scriptIndex + 23) % SubtagSamples.Scripts.Count;
		}

		[Benchmark]
		[InvocationCount(2000000, 200)]
		public LanguageTag L() => LanguageTag.Parse(_textL);

		[Benchmark]
		[InvocationCount(800000, 200)]
		public LanguageTag LSR() => LanguageTag.Parse(_textLSR);

		[Benchmark]
		[InvocationCount(400000, 200)]
		public LanguageTag LSRE() => LanguageTag.Parse(_textLSRE);
	}
}
