using LabSqlParser.AstNode;
namespace LabSqlParser;
interface INodeVisitor {
	void VisitSelect(Select select);
	void VisitIdentifier(Identifier identifier);
	void VisitNumber(Number number);
	void VisitInsert(Insert insert);
	void VisitBinaryOperation(BinaryOperation binaryOperation);
}
