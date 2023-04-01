using LabSqlParser.Visitors;
using System;
using System.Collections.Generic;
namespace LabSqlParser;
static class Program {
	static void Main() {
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		TestLexer();
		TestParser();
		Console.WriteLine(TaskTree.GetTaskTree().ToFormattedString());
		var source = @"INSERT INTO a VALUES ( ( SELECT 1 WHERE 2 ) % ( SELECT 3 ) % 4 / 5 / 6 - 7 = 8 , 9 )";
		var parsedTree = Parser.Parse(Lexer.GetTokens(source));
		Console.WriteLine(parsedTree.ToFormattedString());
		new DebugPrintingVisitor(Console.Out, 2).WriteLine(parsedTree);
	}
	static void TestLexer() {
		static void TestTask() {
			var source = @"INSERT INTO a VALUES ( ( SELECT 1 WHERE 2 ) % ( SELECT 3 ) % 4 / 5 / 6 - 7 = 8 , 9 )";
			var tokens = Lexer.GetTokens(source);
			foreach (var token in tokens) {
				Console.WriteLine(token);
			}
		}
		TestTask();
		var successCases = new List<string> {
			"a A 1 ( ) , % / - =",
			"43 JF",
			"(),()%-",
		};
		var countPassed = 0;
		for (var i = 0; i < successCases.Count; ++i) {
			var source = successCases[i];
			try {
				var tokens1 = Lexer.GetTokens(source);
				foreach (var _ in tokens1) {
					countPassed += 1;
				}
			}
			catch (LexerException e) {
				Console.WriteLine($"Тест {i + 1} не пройден. Запрос `{source}` Ошибка: {e}");
			}
		}
		var failureCases = new List<string> {
			"43!INSERT",
			"asdf &&&123",
			"asdf&&&",
			"&&&asdf",
		};
		for (var i = 0; i < failureCases.Count; ++i) {
			var source = failureCases[i];
			try {
				var tokens1 = Lexer.GetTokens(source);
				foreach (var _ in tokens1) {
					Console.WriteLine();
				}
				Console.WriteLine($"Тест {successCases.Count + i + 1} не пройден. Запрос `{source}` не должен проходить проверку.");
			}
			catch (LexerException) {
				countPassed += 1;
			}
		}
	}
	static void TestParser() {
		var countPassed = 0;
		var successCases = new List<string> {
			"INSERT INTO a VALUES ( 9 )",
			"INSERT INTO a VALUES ( ( SELECT 1 WHERE 2 ) % (( SELECT 3 ) % (4 / 5)) / 6 - (7 = 8) , 9 )",
			"INSERT INTO a VALUES ( ((((((((((9)))))))))) )",
		};
		for (var i = 0; i < successCases.Count; ++i) {
			var source = successCases[i];
			try {
				Parser.Parse(Lexer.GetTokens(source));
				countPassed += 1;
			}
			catch (InvalidOperationException e) {
				Console.WriteLine($"Тест {i + 1} не пройден. Запрос `{source}` Ошибка: {e}");
			}
		}
		var failureCases = new List<string> {
			"INSERT INTO a VALUES (  )",
			"INSERT INTO a VALUES ( ( SELECT 1 2 ) % , 9 )",
			"INSERT INTO a VALUES ( 3 % , 9 )",
			"INSERT INTO a VALUES ( def 9 )",
			"INSERT INTO a VALUES ( , 9 )",
		};
		for (var i = 0; i < failureCases.Count; ++i) {
			var source = failureCases[i];
			try {
				Parser.Parse(Lexer.GetTokens(source));
				Console.WriteLine($"Тест {successCases.Count + i + 1} не пройден. Запрос `{source}` не должен проходить проверку.");
			}
			catch (InvalidOperationException) {
				countPassed += 1;
			}
		}
	}
}
