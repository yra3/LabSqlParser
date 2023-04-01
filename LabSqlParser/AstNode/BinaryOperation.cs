namespace LabSqlParser.AstNode;
sealed record BinaryOperation(
	IExpression Left,
	BinaryOperationType Operator,
	IExpression Right
) : IExpression {
	public string ToFormattedString() {
		var formattedLeft = ExpressionToFormattedString(Left);
		var formattedRight = ExpressionToFormattedString(Right);
		return formattedLeft + " " + BinaryOperatorToFormattedString() + " " + formattedRight;
	}
	static string ExpressionToFormattedString(IExpression expression) {
		return expression.ToFormattedString();
	}
	string BinaryOperatorToFormattedString() {
		switch (Operator) {
			case BinaryOperationType.Division:
				return "/";
			case BinaryOperationType.Equal:
				return "=";
			case BinaryOperationType.Minus:
				return "-";
			case BinaryOperationType.Module:
				return "%";
			default:
				throw new System.NotImplementedException($"Не определено форматирование для оператора: {Operator}");
		}
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitBinaryOperation(this);
	}
}
