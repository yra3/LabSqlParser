using LabSqlParser.AstNode;
using System.Collections.Generic;
namespace LabSqlParser;
static class TaskTree {
	public static INode GetTaskTree() {
		var insert = new Insert(
			new Identifier("a"),
			new List<IExpression> {
				new BinaryOperation(
					new BinaryOperation(
						new BinaryOperation(
							new BinaryOperation(
								new BinaryOperation(
									new BinaryOperation(
										new Select(new Number("1"), new Number("2")),
										BinaryOperationType.Module,
										new Select(new Number("3"), null)
									),
									BinaryOperationType.Module,
									new Number("4")
									),
								BinaryOperationType.Division,
								new Number("5")
								),
							BinaryOperationType.Division,
							new Number("6")
						),
						BinaryOperationType.Minus,
						new Number("7")
					),
					BinaryOperationType.Equal,
					new Number("8")
				),
				new Number("9")
			}
		);
		return insert;
	}
}
