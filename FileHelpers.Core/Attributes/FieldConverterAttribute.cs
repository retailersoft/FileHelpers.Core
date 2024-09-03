using System;
using System.Reflection;
using FileHelpers.Converters;

namespace FileHelpers
{
    /// <summary>Indicates the <see cref="ConverterKind"/> used for read/write operations.</summary>
    /// <remarks>See the <a href="http://www.filehelpers.net/mustread">Complete attributes list</a> for more information and examples of each one.</remarks>

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FieldConverterAttribute : Attribute
    {
        #region "  Constructors  "

        /// <summary>Indicates the <see cref="ConverterKind"/> used for read/write operations. </summary>
        /// <param name="converter">The <see cref="ConverterKind"/> used for the transformations.</param>
        public FieldConverterAttribute(ConverterKind converter)
            : this(converter, new string[] {}) {}

        /// <summary>Indicates the <see cref="ConverterKind"/> used for read/write operations. </summary>
        /// <param name="converter">The <see cref="ConverterKind"/> used for the transformations.</param>
        /// <param name="arg1">The first param passed directly to the Converter Constructor.</param>
        public FieldConverterAttribute(ConverterKind converter, string arg1)
            : this(converter, new string[] {arg1}) {}

        /// <summary>Indicates the <see cref="ConverterKind"/> used for read/write operations. </summary>
        /// <param name="converter">The <see cref="ConverterKind"/> used for the transformations.</param>
        /// <param name="arg1">The first param passed directly to the Converter Constructor.</param>
        /// <param name="arg2">The second param passed directly to the Converter Constructor.</param>
        public FieldConverterAttribute(ConverterKind converter, string arg1, string arg2)
            : this(converter, new string[] {arg1, arg2}) {}

        /// <summary>Indicates the <see cref="ConverterKind"/> used for read/write operations. </summary>
        /// <param name="converter">The <see cref="ConverterKind"/> used for the transformations.</param>
        /// <param name="arg1">The first param passed directly to the Converter Constructor.</param>
        /// <param name="arg2">The second param passed directly to the Converter Constructor.</param>
        /// <param name="arg3">The third param passed directly to the Converter Constructor.</param>
        public FieldConverterAttribute(ConverterKind converter, string arg1, string arg2, string arg3)
            : this(converter, new string[] {arg1, arg2, arg3}) {}


        /// <summary>
        /// Indicates the <see cref="ConverterKind"/> used for read/write operations. 
        /// </summary>
        /// <param name="converter">The <see cref="ConverterKind"/> used for the transformations.</param>
        /// <param name="args">An array of parameters passed directly to the Converter</param>
        private FieldConverterAttribute(ConverterKind converter, params string[] args)
        {
            Kind = converter;
            Type convType = converter switch
            {
                ConverterKind.Date => typeof(DateTimeConverter),
                ConverterKind.DateMultiFormat => typeof(DateTimeMultiFormatConverter),
                ConverterKind.Byte => typeof(ByteConverter),
                ConverterKind.SByte => typeof(SByteConverter),
                ConverterKind.Int16 => typeof(Int16Converter),
                ConverterKind.Int32 => typeof(Int32Converter),
                ConverterKind.Int64 => typeof(Int64Converter),
                ConverterKind.UInt16 => typeof(UInt16Converter),
                ConverterKind.UInt32 => typeof(UInt32Converter),
                ConverterKind.UInt64 => typeof(UInt64Converter),
                ConverterKind.Decimal => typeof(DecimalConverter),
                ConverterKind.Double => typeof(DoubleConverter),
                // Added by Shreyas Narasimhan 17 March 2010
                ConverterKind.PercentDouble => typeof(PercentDoubleConverter),
                ConverterKind.Single => typeof(SingleConverter),
                ConverterKind.Boolean => typeof(BooleanConverter),
                // Added by Alexander Obolonkov 2007.11.08
                ConverterKind.Char => typeof(CharConverter),
                // Added by Alexander Obolonkov 2007.11.08
                ConverterKind.Guid => typeof(GuidConverter),
                _ => throw new BadUsageException("Converter '" + converter.ToString() +
                                                                "' not found, you must specify a valid converter."),
            };
            //mType = type;

            CreateConverter(convType, args);
        }

        /// <summary>Indicates a custom <see cref="ConverterBase"/> implementation.</summary>
        /// <param name="customConverter">The Type of your custom converter.</param>
        /// <param name="arg1">The first param passed directly to the Converter Constructor.</param>
        public FieldConverterAttribute(Type customConverter, string arg1)
            : this(customConverter, new string[] {arg1}) {}

        /// <summary>Indicates a custom <see cref="ConverterBase"/> implementation.</summary>
        /// <param name="customConverter">The Type of your custom converter.</param>
        /// <param name="arg1">The first param passed directly to the Converter Constructor.</param>
        /// <param name="arg2">The second param passed directly to the Converter Constructor.</param>
        public FieldConverterAttribute(Type customConverter, string arg1, string arg2)
            : this(customConverter, new string[] {arg1, arg2}) {}

        /// <summary>Indicates a custom <see cref="ConverterBase"/> implementation.</summary>
        /// <param name="customConverter">The Type of your custom converter.</param>
        /// <param name="arg1">The first param passed directly to the Converter Constructor.</param>
        /// <param name="arg2">The second param passed directly to the Converter Constructor.</param>
        /// <param name="arg3">The third param passed directly to the Converter Constructor.</param>
        public FieldConverterAttribute(Type customConverter, string arg1, string arg2, string arg3)
            : this(customConverter, new string[] {arg1, arg2, arg3}) {}

        /// <summary>Indicates a custom <see cref="ConverterBase"/> implementation.</summary>
        /// <param name="customConverter">The Type of your custom converter.</param>
        /// <param name="args">A list of params passed directly to your converter constructor.</param>
        public FieldConverterAttribute(Type customConverter, params object[] args)
        {
            CreateConverter(customConverter, args);
        }

        /// <summary>Indicates a custom <see cref="ConverterBase"/> implementation.</summary>
        /// <param name="customConverter">The Type of your custom converter.</param>
        public FieldConverterAttribute(Type customConverter)
        {
            CreateConverter(customConverter, new object[] {});
        }

        #endregion

        #region "  Converter  "


        /// <summary>The final concrete converter used for FieldToString and StringToField operations </summary>
        public ConverterBase Converter { get; private set; }

        /// <summary>The <see cref="ConverterKind"/> if a default converter is used </summary>
        public ConverterKind Kind { get; private set; }

        #endregion

        #region "  CreateConverter  "

        private void CreateConverter(Type convType, object[] args)
        {
            if (typeof(ConverterBase).IsAssignableFrom(convType)) {
                ConstructorInfo constructor;
                constructor = convType.GetConstructor(
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    ArgsToTypes(args),
                    null);

                if (constructor == null) {
                    if (args.Length == 0) {
                        throw new BadUsageException("Empty constructor for converter: " + convType.Name +
                                                    " was not found. You must add a constructor without args (can be public or private)");
                    }
                    else {
                        throw new BadUsageException("Constructor for converter: " + convType.Name +
                                                    " with these arguments: (" + ArgsDesc(args) +
                                                    ") was not found. You must add a constructor with this signature (can be public or private)");
                    }
                }

                try {
                    Converter = (ConverterBase)constructor.Invoke(args);
                }
                catch (TargetInvocationException ex) {
                    throw ex.InnerException;
                }
            }
            else if (convType.IsEnum)
                if (args.Length == 0)
                    Converter = new EnumConverter(convType);
                else
                    Converter = new EnumConverter(convType, args[0] as string);

            else
                throw new BadUsageException("The custom converter must inherit from ConverterBase");
        }

        #endregion

        #region "  ArgsToTypes  "

        private static Type[] ArgsToTypes(object[] args)
        {
            if (args == null) {
                throw new BadUsageException(
                    "The args to the constructor can be null if you do not want to pass anything into them.");
            }

            Type[] res = new Type[args.Length];

            for (int i = 0; i < args.Length; i++) {
                if (args[i] == null)
                    res[i] = typeof (object);
                else
                    res[i] = args[i].GetType();
            }

            return res;
        }

        private static string ArgsDesc(object[] args)
        {
            string res = DisplayType(args[0]);

            for (int i = 1; i < args.Length; i++)
                res += ", " + DisplayType(args[i]);

            return res;
        }

        private static string DisplayType(object o)
        {
            if (o == null)
                return "Object";
            else
                return o.GetType().Name;
        }

        #endregion

        internal void ValidateTypes(FieldInfo fi)
        {
            bool valid = false;

            Type fieldType = fi.FieldType;

            if (fieldType.IsValueType &&
                fieldType.IsGenericType &&
                fieldType.GetGenericTypeDefinition() == typeof (Nullable<>))
                fieldType = fieldType.GetGenericArguments()[0];

            switch (Kind) {
                case ConverterKind.None:
                    valid = true;
                    break;

                case ConverterKind.Date:
                case ConverterKind.DateMultiFormat:
                    valid = typeof (DateTime) == fieldType;
                    break;

                case ConverterKind.Byte:
                case ConverterKind.SByte:
                case ConverterKind.Int16:
                case ConverterKind.Int32:
                case ConverterKind.Int64:
                case ConverterKind.UInt16:
                case ConverterKind.UInt32:
                case ConverterKind.UInt64:
                case ConverterKind.Decimal:
                case ConverterKind.Double:
                case ConverterKind.Single:
                case ConverterKind.Boolean:
                case ConverterKind.Char:
                case ConverterKind.Guid:
                    valid = Kind.ToString() == fieldType.UnderlyingSystemType.Name;
                    break;
                case ConverterKind.PercentDouble:
                    valid = typeof (double) == fieldType;
                    break;
            }

            if (valid == false) {
                throw new BadUsageException(
                    "The converter of the field: '" + fi.Name + "' is wrong. The field is of Type: " + fieldType.Name +
                    " and the converter is for type: " + Kind.ToString());
            }
        }
    }
}