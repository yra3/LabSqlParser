namespace LabSqlParser.AstNode;
interface INode {
	string ToFormattedString();
	void AcceptVisitor(INodeVisitor visitor);
}
