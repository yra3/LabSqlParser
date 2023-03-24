namespace LabSqlParser.AstNode;
sealed record Select(
	IExpression Column,
	IExpression? Where
	) : IExpression {
	public string ToFormattedString() {
		var formattedString = $"SELECT {Column.ToFormattedString()}";
		if (Where is not null) {
			formattedString += $" WHERE {Where.ToFormattedString()}";
		}
		return formattedString;
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitSelect(this);
	}
}
