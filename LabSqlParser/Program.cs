using System;
using System.Collections.Generic;
namespace LabSqlParser;
static class Program {
	static void Main() {
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		TestLexer();
		Console.WriteLine(TaskTree.GetTaskTree().ToFormattedString());
		var source = @"INSERT INTO a VALUES ( ( SELECT 1 WHERE 2 ) % ( SELECT 3 ) % 4 / 5 / 6 - 7 = 8 , 9 )";
		var parsedTree = Parser.Parse(Lexer.GetTokens(source));
		Console.WriteLine(parsedTree.ToFormattedString());
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
		var success_cases = new List<string> {
			"a A 1 ( ) , % / - =",
			"43 JF",
			"(),()%-",
		};
		var count_passed = 0;
		for (var i = 0; i < success_cases.Count; ++i) {
			var source = success_cases[i];
			try {
				var tokens1 = Lexer.GetTokens(source);
				foreach (var _ in tokens1) {
					count_passed += 1;
				}
			}
			catch (LexerException e) {
				Console.WriteLine($"Тест {i + 1} не пройден. Запрос `{source}` Ошибка: {e}");
			}
		}
		var failure_cases = new List<string> {
			"43INSERT",
			"asdf &&&123",
			"asdf&&&",
			"&&&asdf",
		};
		for (var i = 0; i < failure_cases.Count; ++i) {
			var source = failure_cases[i];
			try {
				var tokens1 = Lexer.GetTokens(source);
				foreach (var _ in tokens1) {
					Console.WriteLine();
				}
				Console.WriteLine($"Тест {success_cases.Count + i + 1} не пройден. Запрос `{source}` не должен проходить проверку.");
			}
			catch (LexerException) {
				count_passed += 1;
			}
		}
	}
}
