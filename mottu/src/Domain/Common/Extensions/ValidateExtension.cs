using Domain.Common.Enums;
using System;
using System.Text.RegularExpressions;

namespace Domain.Common.Extensions;

public static class ValidateExtension
{
    public static bool IsCnpjValid(
        string cnpj)
    {
        cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

        if (cnpj.Length != 14 || !long.TryParse(cnpj, out _))
            return false;

        int[] listWeightOne = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int sumOne = 0;

        for (int i = 0; i < 12; i++)
        {
            sumOne += (cnpj[i] - '0') * listWeightOne[i];
        }

        int remainderOne = sumOne % 11;

        int digitOne = remainderOne < 2 ? 0 : 11 - remainderOne;

        int[] listWeightsTwo = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int sumTwo = 0;

        for (int i = 0; i < 13; i++)
        {
            sumTwo += (cnpj[i] - '0') * listWeightsTwo[i];
        }

        int remainderTwo = sumTwo % 11;

        int digitTwo = remainderTwo < 2 ? 0 : 11 - remainderTwo;

        return cnpj[12] - '0' == digitOne && cnpj[13] - '0' == digitTwo;
    }

    public static bool IsCnhValid(
        string cnh)
    {
        cnh = cnh.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

        if (!Regex.IsMatch(cnh, @"^\d{11}$"))
            return false;

        if (new Regex(@"^(\d)\1{10}$").IsMatch(cnh))
            return false;

        return true;
    }

    public static bool IsCnhTypeValid(
        string input,
        out CnhTypeEnum cnhType)
    {
        return Enum.TryParse(input, true, out cnhType) &&
               Enum.IsDefined(typeof(CnhTypeEnum), cnhType);
    }
}