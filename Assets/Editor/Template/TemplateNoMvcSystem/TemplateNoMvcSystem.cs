using Framework;
namespace GameSystem.TemplateNoMvcSystem
{
    public interface ITemplateNoMvcSystemModule: IModule
    {
        
    }

    public class TemplateNoMvcSystemModule : AbsModule, ITemplateNoMvcSystemModule
    {
        protected override void OnInit()
        {
        }
    }
}