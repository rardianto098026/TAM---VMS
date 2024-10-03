

using TAM.VMS.Domain.Object;

namespace TAM.VMS.Infrastructure.General
{
    public interface IGeneralHelper
    {
        ConfigObject? GetDataConfig();
        string GenerateID();
        string RemoveSpecialCharacters(string value);
        bool OnlyNumberCharacter(string value);
        bool OnlyNumber(string value);
        string NumberToRupiah(decimal angka);
        string NumberToRupiahWithoutPrefix(decimal angka);
        string RemoveSpecialChar(string input);
    }
}

