using System.Collections.Generic;
using UnityEngine;
using System;

public class BugReporter : MonoBehaviour
{
    private static Queue<string> logLines = new Queue<string>();
    private const int maxLogLines = 50;

    void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private static void HandleLog(string logString, string stackTrace, LogType type)
    {
        DateTime now = DateTime.Now;
        TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(now);
        string offsetString = offset >= TimeSpan.Zero
            ? $"+{offset:hh\\:mm}"
            : $"-{offset:hh\\:mm}";

        string timestamp = now.ToString($"hh:mm:ss tt on MM-dd-yyyy '{offsetString}'");
        string logEntry = $"[{timestamp}] [{type}] {logString}";
        if (type == LogType.Exception || type == LogType.Error)
            logEntry += $"\n{stackTrace}";

        if (logLines.Count >= maxLogLines)
            logLines.Dequeue();
        logLines.Enqueue(logEntry);
    }

    public void OpenBugReportEmail()
    {
        string email = "simeckentertainment@gmail.com";
        string subject = EscapeURL("Bug report for Fat Butters Jetpack Ride");

        string logDump = string.Join("\n", logLines.ToArray());

        string body = EscapeURL(
@"What were you doing?
(Describe the action you were taking)

What did you expect to happen?

What actually happened?



------------------------------
Debug Info (auto-filled)
------------------------------
Device Model: " + SystemInfo.deviceModel + @"
Device OS: " + SystemInfo.operatingSystem + @"
Platform: " + Application.platform + @"
Game Version: " + Application.version + @"
Unity Version: " + Application.unityVersion + @"
Language: " + Application.systemLanguage + @"
Timestamp: " + DateTime.Now.ToString("hh:mm:ss tt on MM-dd-yyyy") + @"


------------------------------
Recent Log Output (last 50 lines)
------------------------------
--- START OF LOGS ---
" + logDump + @"
--- END OF LOGS ---
"
        );

        Application.OpenURL($"mailto:{email}?subject={subject}&body={body}");
    }

    private string EscapeURL(string text)
    {
        return WWW.EscapeURL(text).Replace("+", "%20");
    }
}
