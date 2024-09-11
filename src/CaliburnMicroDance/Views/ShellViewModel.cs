using Caliburn.Micro;

namespace CaliburnMicroDance;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
{
    public ShellViewModel()
    {
        DisplayName = "CM dance. ❤❤";

        var s1 = new StudentViewModel() { DisplayName = "stu sam" };

        EnsureItem(s1);
        EnsureItem(new TeacherViewModel() { DisplayName = "teacher john" });

        ActiveItem = s1;
    }
}