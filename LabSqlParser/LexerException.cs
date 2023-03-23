using System;
namespace LabSqlParser;
sealed class LexerException : Exception {
	public LexerException(string info) : base(info) { }
}
