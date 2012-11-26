// -----------------------------------------------------------------------
// <copyright file="AK_IBAN_validation.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace nomemTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// AK, IBAN Validation (old version)
    /// TODO: review and rewrite
    /// </summary>
    public static partial class winForm_treeViewPopulation
    {
        #region AK validacija

        public static bool IsAsmensKodasGood(string asmensKodas)
        {
            if (String.IsNullOrEmpty(asmensKodas))
            {
                return false;
            }
            else
            {
                asmensKodas = WhiteSpaceRemove(asmensKodas);

                if (CheckIfForeigner(asmensKodas))
                {
                    return true;
                }

                if (!CheckIfNumerable(asmensKodas))
                {
                    return false;
                }

                else
                {
                    int L, Y1, Y2, M1, M2, D1, D2, X1, X2, X3, K = 0;
                    L = int.Parse(asmensKodas[0].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    Y1 = int.Parse(asmensKodas[1].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    Y2 = int.Parse(asmensKodas[2].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    M1 = int.Parse(asmensKodas[3].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    M2 = int.Parse(asmensKodas[4].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    D1 = int.Parse(asmensKodas[5].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    D2 = int.Parse(asmensKodas[6].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    X1 = int.Parse(asmensKodas[7].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    X2 = int.Parse(asmensKodas[8].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    X3 = int.Parse(asmensKodas[9].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                    K = int.Parse(asmensKodas[10].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

                    // Tikrinamas pirmas skaitmuo ar yra nuo 1 iki 6
                    if (L == 0)
                    {
                        return false;
                    }
                    else if (L == 7)
                    {
                        return false;
                    }
                    else if (L == 8)
                    {
                        return false;
                    }
                    else if (L == 9)
                    {
                        return false;
                    }

                    // Tikrina datas
                    else if (int.Parse(Y1.ToString(CultureInfo.InvariantCulture) + Y2.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) < 00 || int.Parse(Y1.ToString(CultureInfo.InvariantCulture) + Y2.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) > 99)
                    {
                        return false;
                    }
                    else if (int.Parse(M1.ToString(CultureInfo.InvariantCulture) + M2.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) < 01 || int.Parse(M1.ToString(CultureInfo.InvariantCulture) + M2.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) > 12)
                    {
                        return false;
                    }
                    else if (int.Parse(D1.ToString(CultureInfo.InvariantCulture) + D2.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) < 01 || int.Parse(D1.ToString(CultureInfo.InvariantCulture) + D2.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) > 31)
                    {
                        return false;
                    }

                    // Tikrina kontroline suma
                    else
                    {
                        int S = 0, Liekana = 0;
                        S = L * 1 + Y1 * 2 + Y2 * 3 + M1 * 4 + M2 * 5 + D1 * 6 + D2 * 7 + X1 * 8 + X2 * 9 + X3 * 1;
                        Liekana = S % 11;
                        if (Liekana != 10 && Liekana == K)
                        {
                            return true;
                        }
                        else if (Liekana == 10)
                        {
                            S = 0;
                            S = L * 3 + Y1 * 4 + Y2 * 5 + M1 * 6 + M2 * 7 + D1 * 8 + D2 * 9 + X1 * 1 + X2 * 2 + X3 * 3;
                            Liekana = S % 11;
                            if (Liekana != 10 && Liekana == K)
                            {
                                return true;
                            }
                            else if (Liekana == 10 && K == 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        private static string WhiteSpaceRemove(string a)
        {
            a = a.Trim();
            string temp = string.Empty;
            int i;
            for (i = 0; i < a.Length; i++)
                if (a[i] != ' ') temp += a[i];

            a = temp;
            return a;
        }

        private static bool CheckIfNumerable(string a)
        {
            Regex check = new Regex("^[0-9]{11}$");
            return check.IsMatch(a);
        }

        private static bool CheckIfForeigner(string code)
        {
            Regex check = new Regex("^[0-9]{7}[xX]{4}$");
            return check.IsMatch(code);
        }

        #endregion

        #region IBAN validacija

        public static bool IsIBANGood(string strIBAN)
        {
            string IBAN = "^LT[0-9]{18}$";
            Match IBANMatch = Regex.Match(strIBAN, IBAN);
            if (!IBANMatch.Success)
            {
                return false;
            }
            int intASC;
            string strIBAN2, strIBAN3;
            strIBAN3 = null;
            strIBAN2 = Right(strIBAN, 16) + Left(strIBAN, 4);
            for (int i = 0; i < 20; i++)
            {
                intASC = Convert.ToInt32(strIBAN2[i], CultureInfo.InvariantCulture);
                if (intASC >= 65 && intASC <= 90)
                {
                    strIBAN3 += intASC - 55;
                }
                else
                {
                    strIBAN3 += strIBAN2[i];
                }
            }

            //IsNumeric tikrina dalimis visa saskaitos numeri, nes neimanoma (per ilgas) tikrinti viso (.net 2.0)
            if (IsNumeric(strIBAN3.Substring(0, 11)) == true &&
                IsNumeric(strIBAN3.Substring(11, strIBAN3.Length - 11)) == true)
            {
                //LongDivision("97",strIBAN3,strRetWhole, strRetRemain);
                if (IlgaDalyba(strIBAN3) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool IlgaDalyba(string kodas)
        {
            string kodas_dalis1, kodas_dalis2, liekana_string = null;
            long kodas1_sk, kodas2_sk = 0;
            long daliklis = 97;
            long liekana = 0;

            //suskaido IBAN is 22 skaitmenu i dvi dalis 
            kodas_dalis1 = kodas.Substring(0, 11);
            kodas_dalis2 = kodas.Substring(11, kodas.Length - 11);

            //Pirma dali pavercia is string, i skaitciu
            kodas1_sk = Convert.ToInt64(kodas_dalis1, CultureInfo.InvariantCulture);

            //Paskaiciuoja pirmos dalies liekana
            liekana = kodas1_sk % daliklis;
            liekana_string = liekana.ToString(CultureInfo.InvariantCulture);

            //Liekana prideda prie antros dalies pradzio
            kodas_dalis2 = liekana_string + kodas_dalis2;
            kodas2_sk = Convert.ToInt64(kodas_dalis2, CultureInfo.InvariantCulture);

            //Skaiciuoja antros dalies liekana, kuri jei IBAN teisingas turi buti =1
            liekana = kodas2_sk % daliklis;

            //MessageBox.Show(kodas + "\n" + kodas_dalis1 + "\n" + kodas_dalis2 + "\n" + liekana_string);

            if (liekana == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }

        private static string Right(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }

        private static string Mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex);
            return result;
        }

        private static string Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex, length);
            return result;
        }

        private static bool IsNumeric(string theValue)
        {
            try
            {
                Convert.ToInt64(theValue, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
