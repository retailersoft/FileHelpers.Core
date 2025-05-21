using System.Reflection;


public abstract class EnumEnumeration<TEnum, TBackingEnum> : Enumeration<TEnum>
    where TEnum : EnumEnumeration<TEnum, TBackingEnum>
    where TBackingEnum : struct, Enum
{
    public static bool AllowUnknown { get; set; } = true;

    public static TEnum Unknown { get; } = CreateUnknown();

    protected EnumEnumeration(int value, string name) : base(value, name) { }

    public static TEnum? FromEnum(TBackingEnum? enumValue)
    {
        if (enumValue is null)
            return AllowUnknown ? Unknown : default;

        int intValue = Convert.ToInt32(enumValue.Value);

        if (Enumerations.TryGetValue(intValue, out TEnum? existing))
            return existing;

        if (!AllowUnknown)
            return default;

        string name = enumValue.ToString() ?? intValue.ToString();

        var instance = CreateInstance(intValue, name);
        Enumerations[intValue] = instance;

        return instance;
    }

    public static void RegisterAll()
    {
        foreach (TBackingEnum value in Enum.GetValues(typeof(TBackingEnum)))
        {
            _ = FromEnum(value);
        }
    }

    private static TEnum CreateUnknown()
    {
        return CreateInstance(0, "Unknown");
    }

    private static TEnum CreateInstance(int value, string name)
    {
        var ctor = typeof(TEnum).GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            types: new[] { typeof(int), typeof(string) },
            modifiers: null);

        if (ctor is null)
            throw new InvalidOperationException(
                $"Missing constructor (int, string) in {typeof(TEnum).Name}");

        return (TEnum)ctor.Invoke(new object[] { value, name });
    }
}


public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    protected static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public int Value { get; protected init; }

    public string Name { get; protected init; }

    //public static TEnum? FromEnum<T>(T value) where T : Enum
    //{
    //    int intValue = Convert.ToInt32(value);
    //    string name = value.ToString();

    //    if (Enumerations.TryGetValue(intValue, out TEnum? existing))
    //        return existing;

    //    // Dynamically create the missing instance
    //    var enumType = typeof(TEnum);
    //    var ctor = enumType.GetConstructor(
    //        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
    //        binder: null,
    //        types: new[] { typeof(int), typeof(string) },
    //        modifiers: null);

    //    if (ctor is null)
    //        throw new InvalidOperationException(
    //            $"Cannot create a new instance of {enumType.Name}. Missing constructor (int, string).");

    //    var newEnum = (TEnum)ctor.Invoke(new object[] { intValue, name });

    //    // Register it in the dictionary
    //    Enumerations[intValue] = newEnum;

    //    return newEnum;
    //}


    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(
            value,
            out TEnum? enumeration) ?
                enumeration :
                default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(e => e.Name == name);
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() &&
               Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Value);
    }
}