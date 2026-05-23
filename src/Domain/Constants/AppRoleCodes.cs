namespace Domain.Constants;

/// <summary>業務ロール（設計書 5.9）</summary>
public static class AppRoleCodes
{
    public const string Guarantee = "Guarantee";
    public const string Admin = "Admin";
    public const string Accounting = "Accounting";
    public const string Promotion = "Promotion";
    public const string All = "All";

    public static readonly IReadOnlyList<string> AllRoles =
        [Guarantee, Admin, Accounting, Promotion, All];
}
