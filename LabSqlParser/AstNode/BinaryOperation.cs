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
	string ExpressionToFormattedString(IExpression expression) {
		var isLowPreсedence = false;
		if (expression is BinaryOperation operation) {
			var innerOperator = operation.Operator;
			isLowPreсedence = OperationPreсedence.GetPreсedence(innerOperator) < OperationPreсedence.GetPreсedence(Operator);
		}
		if (isLowPreсedence || expression is Select) {
			return $"( {expression.ToFormattedString()} )";
		}
		else {
			return expression.ToFormattedString();
		}
	}
	public string BinaryOperatorToFormattedString() {
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
