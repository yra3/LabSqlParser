using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace LabSqlParser;
static partial class Lexer {
	public static IEnumerable<Token> GetTokens(string input) {
		var matches = GetMatches(LexemeRx(), input);
		var lastPos = 0;
		foreach (var m in matches) {
			if (m.Index != lastPos) {
				throw new LexerException($"Нераспознанный токен на интервале ({lastPos}, {m.Index - 1}):  `{input[lastPos..m.Index]}`");
			}
			CheckUniqueMatchedGroupe(m);
			if (m.Groups["spaces"].Success) {
				yield return new Token(TokenType.Spaces, m.Value);
			}
			else if (m.Groups["identifier"].Success) {
				yield return new Token(TokenType.Identifier, m.Value);
			}
			else if (m.Groups["punctuator"].Success) {
				yield return new Token(TokenType.Punctuator, m.Value);
			}
			else if (m.Groups["number"].Success) {
				yield return new Token(TokenType.Number, m.Value);
			}
			else {
				throw new LexerException("Неверное имя группы");
			}
			lastPos += m.Length;
		}
		if (lastPos != input.Length) {
			throw new LexerException($"Нераспознанный токен на интервале ({lastPos}, {input.Length - 1}):  `{input[lastPos..input.Length]}`");
		}
	}
	static IEnumerable<Match> GetMatches(Regex rx, string input) {
		var match = rx.Match(input);
		while (match.Success) {
			yield return match;
			match = match.NextMatch();
		}
	}
	static void CheckUniqueMatchedGroupe(Match m) {
		var countGroups = -1;
		foreach (var group in m.Groups.Values) {
			if (group.Success) {
				countGroups += 1;
			}
		}
		if (countGroups != 1) {
			throw new LexerException($"Неоднозначный тип токена. Лексема: `{m.Value}`, Группы: {m.Groups.GetEnumerator()}");
		}
	}
	[GeneratedRegex("""
		(?<spaces>[ \t\r\n])|
		(?<identifier>[A-Za-z_][A-Za-z0-9_]*)|
		(?<number>[0-9]+)|
		(?<punctuator>[,()=/%-])
		""", RegexOptions.IgnorePatternWhitespace)]
	private static partial Regex LexemeRx();
}
