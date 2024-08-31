using UnityEngine;
using UnityEngine.UI;

public class TaskDisplayer : MonoBehaviour
{
    [SerializeField] private Text Text;

    [SerializeField] private TaskInfo[] Tasks;

    public void ApplyTask(TaskInfo info, bool remove)
    {
        if (remove)
        {
            Tasks = StaticTools.RemoveFromMassive(Tasks, info);
        }
        else
        {
            Tasks = StaticTools.ExcludingExpandMassive(Tasks, info);
        }

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if(Tasks.Length == 0)
        {
            Text.text = "";
            return;
        }

        int high = 0;
        for(int i = 1; i < Tasks.Length; i++)
        {
            if (Tasks[high].Priority < Tasks[i].Priority)
            {
                high = i;
            }
        }

        Text.text = Tasks[high].Info;
    }
}

[System.Serializable]
public class TaskInfo
{
    [Multiline] public string Info;
    public int Priority;

    public TaskInfo(string info, int priority)
    {
        Info = info;
        Priority = priority;
    }
}
