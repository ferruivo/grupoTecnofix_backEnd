using System.Text.RegularExpressions;

namespace GrupoTecnofix_Api.Utils
{
    public static class DocumentoUtils
    {
        public static string OnlyDigits(string? s)
            => string.IsNullOrWhiteSpace(s) ? "" : Regex.Replace(s, @"\D", "");

        public static bool IsValidCpf(string cpf)
        {
            cpf = OnlyDigits(cpf);
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], 11) == cpf) return false;

            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var temp = cpf[..9];
            var sum = 0;
            for (int i = 0; i < 9; i++) sum += (temp[i] - '0') * mult1[i];
            var mod = sum % 11;
            var dig1 = mod < 2 ? 0 : 11 - mod;

            temp += dig1;
            sum = 0;
            for (int i = 0; i < 10; i++) sum += (temp[i] - '0') * mult2[i];
            mod = sum % 11;
            var dig2 = mod < 2 ? 0 : 11 - mod;

            return cpf.EndsWith($"{dig1}{dig2}");
        }

        public static bool IsValidCnpj(string cnpj)
        {
            cnpj = OnlyDigits(cnpj);
            if (cnpj.Length != 14) return false;
            if (new string(cnpj[0], 14) == cnpj) return false;

            int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var temp = cnpj[..12];
            var sum = 0;
            for (int i = 0; i < 12; i++) sum += (temp[i] - '0') * mult1[i];
            var mod = sum % 11;
            var dig1 = mod < 2 ? 0 : 11 - mod;

            temp += dig1;
            sum = 0;
            for (int i = 0; i < 13; i++) sum += (temp[i] - '0') * mult2[i];
            mod = sum % 11;
            var dig2 = mod < 2 ? 0 : 11 - mod;

            return cnpj.EndsWith($"{dig1}{dig2}");
        }
    }
}
