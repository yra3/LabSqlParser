using LabSqlParser.AstNode;
using System;
namespace LabSqlParser;
public static class OperationPreсedence {
	public static int GetPreсedence(BinaryOperationType operationType) {
		return operationType switch {
			BinaryOperationType.Division => 30,
			BinaryOperationType.Module => 30,
			BinaryOperationType.Minus => 20,
			BinaryOperationType.Equal => 10,
			_ => throw new NotImplementedException($"Не задан приоритет операции: {operationType}"),
		};
	}
}
