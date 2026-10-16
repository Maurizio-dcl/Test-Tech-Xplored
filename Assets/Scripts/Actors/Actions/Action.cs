namespace Actors.Actions
{
    public class Action
    {
        public ActionDefinition Definition { get; }
        public IActionLogic Logic { get; }
        
        public Action(ActionDefinition definition, IActionLogic logic)
        {
            Definition = definition;
            Logic = logic;
        }
        
        public void Execute() => Logic.Execute(Definition);
    }
}