using SpiderTool.Data.Dto.Tasks;

namespace SpiderWin.Constants
{
    public class ConstantsVariable
    {
        public static Color CompletedColor = Color.FromArgb(80, 233, 200);
        public static Color InProgressColor = Color.FromArgb(130, 240, 150);
        public static Color CanceledColor = Color.FromArgb(230, 140, 150);
        public static Dictionary<TaskType, Color> TaskColor = new Dictionary<TaskType, Color>()
        {
            { TaskType.Completed, CompletedColor },
            { TaskType.InProgress, InProgressColor },
            { TaskType.Canceled, CanceledColor },
            { TaskType.NotEffective, Color.WhiteSmoke }
        };
    }
}
