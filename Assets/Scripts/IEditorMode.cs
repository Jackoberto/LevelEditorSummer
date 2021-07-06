public interface IEditorMode
{
    string Name { get; }
    void Initialize(Editor editor);
    void Enter();
    void Exit();
    void EditorUpdate();
}