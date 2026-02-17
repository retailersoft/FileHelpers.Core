# FileHelpers.Core

A powerful and easy-to-use .NET library for importing and exporting data from fixed-length or delimited files, strings, or streams.

## Features

- **Declarative Mapping**: Use attributes to define file structure
- **Multiple Formats**: Support for CSV, fixed-length, and custom delimited formats
- **Type Conversion**: Automatic conversion between file data and .NET types
- **Error Handling**: Robust error management and validation
- **Async Support**: Asynchronous reading and writing for large files
- **Performance**: Optimized for speed and memory efficiency

## Installation

```bash
dotnet add package FileHelpers.Core
```

## Quick Start

### Reading a CSV File

```csharp
using FileHelpers.Core;

// Define your record class
[DelimitedRecord(",")]
public class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime RegisterDate { get; set; }
}

// Read the file
var engine = new FileHelperEngine<Customer>();
var records = engine.ReadFile("customers.csv");

foreach (var customer in records)
{
    Console.WriteLine($"{customer.Name} - {customer.Email}");
}
```

### Writing to a File

```csharp
var customers = new List<Customer>
{
    new Customer { Name = "John Doe", Email = "john@example.com", RegisterDate = DateTime.Now },
    new Customer { Name = "Jane Smith", Email = "jane@example.com", RegisterDate = DateTime.Now }
};

var engine = new FileHelperEngine<Customer>();
engine.WriteFile("output.csv", customers);
```

### Fixed-Length Records

```csharp
[FixedLengthRecord]
public class Product
{
    [FieldFixedLength(10)]
    public string SKU { get; set; }
    
    [FieldFixedLength(30)]
    public string Description { get; set; }
    
    [FieldFixedLength(8)]
    [FieldConverter(ConverterKind.Decimal)]
    public decimal Price { get; set; }
}
```

## Advanced Features

### Custom Converters

```csharp
[DelimitedRecord(",")]
public class Order
{
    public int OrderId { get; set; }
    
    [FieldConverter(typeof(MyCustomDateConverter))]
    public DateTime OrderDate { get; set; }
}
```

### Error Handling

```csharp
var engine = new FileHelperEngine<Customer>();
engine.ErrorMode = ErrorMode.SaveAndContinue;

var records = engine.ReadFile("data.csv");

if (engine.ErrorManager.HasErrors)
{
    foreach (var error in engine.ErrorManager.Errors)
    {
        Console.WriteLine($"Line {error.LineNumber}: {error.ExceptionInfo.Message}");
    }
}
```

### Async Reading

```csharp
var engine = new FileHelperAsyncEngine<Customer>();

using (engine.BeginReadFile("large-file.csv"))
{
    await foreach (var customer in engine.ReadNextAsync())
    {
        // Process each record
    }
}
```

## Supported Formats

- **CSV** - Comma-separated values
- **TSV** - Tab-separated values
- **Fixed-Length** - Fixed-width column files
- **Custom Delimited** - Any custom delimiter
- **Multi-Record** - Files with multiple record types

## .NET Version Support

- .NET 9.0
- Built with modern C# features and performance optimizations

## About This Fork

This is a maintained fork of the FileHelpers library, targeting .NET 9. The original FileHelpers library was created by Marcos Meli.

### Why This Fork?

- Updated for .NET 9
- Maintained for modern .NET applications
- Compatible with latest C# features
- Active development and bug fixes

## Documentation

For more examples and detailed documentation, visit:
- [GitHub Repository](https://github.com/retailersoft/FileHelpers.Core)
- [Original FileHelpers Documentation](http://www.filehelpers.net)

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## License

MIT License - See LICENSE file for details

Original FileHelpers library: Copyright (c) Marcos Meli - Devoo  
This fork: Maintained by RetailerSoft

## Support

- Issues: https://github.com/retailersoft/FileHelpers.Core/issues
- Original Project: http://www.filehelpers.net

## Examples

### Skip Header Rows

```csharp
[DelimitedRecord(",")]
[IgnoreFirst(1)]  // Skip header row
public class DataRecord
{
    public string Column1 { get; set; }
    public string Column2 { get; set; }
}
```

### Conditional Record Reading

```csharp
var engine = new FileHelperEngine<Customer>();
var records = engine.ReadFile("data.csv")
    .Where(c => c.RegisterDate > DateTime.Now.AddYears(-1));
```

### Writing with Custom Header

```csharp
var engine = new FileHelperEngine<Customer>();
engine.HeaderText = "Name,Email,Registration Date";
engine.WriteFile("output.csv", customers);
```

## Performance Tips

- Use `FileHelperAsyncEngine` for large files
- Set `ErrorMode.IgnoreAndContinue` if you don't need error details
- Consider using streaming for memory-intensive operations
- Cache engine instances when processing multiple files

## Common Scenarios

### Import Excel/CSV Data
Perfect for importing data from spreadsheets into your application.

### Export Reports
Generate CSV files for reporting and data exchange.

### Data Migration
Move data between systems using flat files.

### ETL Operations
Extract, transform, and load data pipelines.

### Log File Parsing
Parse structured log files into .NET objects.

---

**FileHelpers.Core** - Making file I/O simple and elegant for .NET developers! ??
