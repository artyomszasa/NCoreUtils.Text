using NCoreUtils.Text;

namespace NCoreUtils;

public static class NamingConvention
{
    public static CamelCaseNamingConvention CamelCase { get; } = new CamelCaseNamingConvention();

    public static KebabCaseNamingConvention KebabCase { get; } = new KebabCaseNamingConvention();

    public static PascalCaseNamingConvention PascalCase { get; } = new PascalCaseNamingConvention();

    public static SnakeCaseNamingConvention SnakeCase { get; } = new SnakeCaseNamingConvention();
}