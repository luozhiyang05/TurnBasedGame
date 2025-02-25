using Framework;
namespace Assets.GameSystem.SkillSystem
{
    public interface ISkillSystemModule: IModule
    {
        
    }

    public class SkillSystemModule : AbsModule, ISkillSystemModule
    {
        protected override void OnInit()
        {
        }
    }
}