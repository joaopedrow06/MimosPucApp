namespace WebUi.Pages;

public static class Utilities
{
    public static string FillCardChipIlist(this IList<string> list)
    {
        return list.Count > 0 ? $"{list.Count}" : string.Empty;
    }
}
