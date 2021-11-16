using System.Globalization;

namespace Domain.Interfaces
{
    public interface IStringConvertible
    {
        string ToOutputString(CultureInfo cultureInfo);
    }
}