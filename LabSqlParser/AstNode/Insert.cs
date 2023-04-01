using System.Collections.Generic;
using System.Linq;
namespace LabSqlParser.AstNode;
sealed record Insert(
	Identifier Into,
	IReadOnlyList<IExpression> Values
) : INode {
	public string ToFormattedString() {
		var formattedString = $"INSERT INTO {Into.ToFormattedString()} VALUES ( ";
		formattedString += string.Join(" , ", Values.Select(value => value.ToFormattedString()));
		formattedString += " )";
		return formattedString;
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitInsert(this);
	}
}
