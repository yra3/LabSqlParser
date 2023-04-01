namespace LabSqlParser.AstNode;
sealed record Identifier(
	string Lexeme
) : IExpression {
	public string ToFormattedString() {
		return Lexeme;
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitIdentifier(this);
	}
}
