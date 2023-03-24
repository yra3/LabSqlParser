using LabSqlParser.AstNode;
using System.IO;
namespace LabSqlParser.Visitors;
sealed class DebugPrintingVisitor : INodeVisitor {
	readonly TextWriter output;
	int indent;
	public DebugPrintingVisitor(TextWriter output, int indent = 0) {
		this.output = output;
		this.indent = indent;
	}
	public void WriteLine(INode node) {
		WriteNode(node);
		Write("\n");
	}
	void WriteNode(INode node) {
		node.AcceptVisitor(this);
	}
	void Write(string s) {
		output.Write(s);
	}
	void WriteIndent() {
		Write(new string('\t', indent));
	}
	void INodeVisitor.VisitSelect(Select select) {
		Write($"new {nameof(Select)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(select.Column);
			Write(",\n");
			WriteIndent();
			if (select.Where is not null) {
				WriteNode(select.Where);
			}
			else {
				Write("null");
			}
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
	void INodeVisitor.VisitIdentifier(Identifier identifier) {
		Write($"new {nameof(Identifier)}(\"{identifier.Lexeme}\")");
	}
	void INodeVisitor.VisitNumber(Number number) {
		Write($"new {nameof(Number)}(\"{number.Lexeme}\")");
	}
	void INodeVisitor.VisitBinaryOperation(BinaryOperation binaryOperation) {
		Write($"new {nameof(BinaryOperation)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(binaryOperation.Left);
			Write(",\n");
			WriteIndent();
			Write(nameof(BinaryOperationType) + "." + binaryOperation.Operator.ToString());
			Write(",\n");
			WriteIndent();
			WriteNode(binaryOperation.Right);
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
	void INodeVisitor.VisitInsert(Insert insert) {
		Write($"new {nameof(Insert)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(insert.Into);
			Write(",\n");
			WriteIndent();
			Write($"new List<{nameof(IExpression)}> {{\n");
			{
				indent += 1;
				foreach (var value in insert.Values) {
					WriteIndent();
					WriteNode(value);
					Write(",\n");
				}
				indent -= 1;
			}
			WriteIndent();
			Write("}\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
}
