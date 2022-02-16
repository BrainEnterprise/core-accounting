using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BrainEnterprise.Core.Accounting.Vat
{
    /// <summary>
    /// Helper class for TaxCode in various Country
    /// </summary>
    public static class TaxCodeHelper
    {
        /// <summary>
        /// Specific Utilities for Italian Fiscal Code
        /// </summary>
        public static class Italian
        {
            private static readonly string _months = "ABCDEHLMPRST";
            private static readonly string _vocals = "AEIOU";
            private static readonly string _consonants = "BCDFGHJKLMNPQRSTVWXYZ";
            private static readonly string _omocodeChars = "LMNPQRSTUV";
            private static readonly int[] _controlCodeArray = new[] { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
            private static readonly Regex _checkRegEx = new Regex(@"^[A-Z]{6}[\d]{2}[A-Z][\d]{2}[A-Z][\d]{3}[A-Z]$");

            /// <summary>
            /// Effettua varie operazioni di normalizzazione su una stringa, rimuovendo spazi e/o caratteri non utilizzati.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="normalizeDiacritics">TRUE per sostituire le lettere accentate con il loro equivalente non accentato</param>
            /// <returns></returns>
            private static string _normalize(string s, bool normalizeDiacritics)
            {
                if (String.IsNullOrEmpty(s)) return s;
                s = s.Trim().ToUpper();
                if (normalizeDiacritics)
                {
                    string src = "ÀÈÉÌÒÙàèéìòù";
                    string rep = "AEEIOUAEEIOU";
                    for (int i = 0; i < src.Length; i++) s = s.Replace(src[i], rep[i]);
                    return s;
                }
                return s;
            }

            /// <summary>
            /// Sostituisce le lettere utilizzate per modificare il Codice Fiscale in caso di omocodia (se presenti) con i relativi numeri.
            /// </summary>
            /// <param name="fiscalCode">Fiscal Code potentially containing omocode chars</param>
            /// <returns>Il Codice Fiscale epurato dalle eventuali modifiche dovute a casi di omocodia (da utilizzare per il calcolo di nome, cognome et. al.)</returns>
            private static string _omocodeReplace(string fiscalCode)
            {
                char[] cfChars = fiscalCode.ToCharArray();
                int[] pos = new[] { 6, 7, 9, 10, 12, 13, 14 };
                foreach (int i in pos) if (!Char.IsNumber(cfChars[i])) cfChars[i] = _omocodeChars.IndexOf(cfChars[i]).ToString()[0];
                return new string(cfChars);
            }

            /// <summary>
            /// Calcola il carattere di controllo sulla base dei precedenti 15 caratteri del Codice Fiscale.
            /// </summary>
            /// <param name="f15">I primi 15 caratteri del Codice Fiscale (ovvero tutti tranne il Carattere di Controllo)</param>
            /// <returns>Il carattere di controllo da utilizzare per il calcolo del Codice Fiscale</returns>
            private static char _calculateCheckDigit(string f15)
            {
                int tot = 0;
                byte[] arrCode = Encoding.ASCII.GetBytes(f15.ToUpper());
                for (int i = 0; i < f15.Length; i++)
                    if ((i + 1) % 2 == 0) tot += (char.IsLetter(f15, i))
                        ? arrCode[i] - (byte)'A'
                        : arrCode[i] - (byte)'0';
                    else tot += (char.IsLetter(f15, i))
                        ? _controlCodeArray[(arrCode[i] - (byte)'A')]
                        : _controlCodeArray[(arrCode[i] - (byte)'0')];
                tot %= 26;
                char l = (char)(tot + 'A');
                return l;
            }

            /// <summary>
            /// Calcola le 5 lettere relative a data di nascita e genere, utilizzate per il calcolo del Codice Fiscale.
            /// </summary>
            /// <param name="dateOfBirth">La data di nascita</param>
            /// <param name="genre">Il genere ('M' o 'F')</param>
            /// <returns>Le 5 lettere che saranno utilizzate per il calcolo del Codice Fiscale.</returns>
            private static string _calculateDateOfBirthGenre(DateTime dateOfBirth, char genre)
            {
                string code = dateOfBirth.Year.ToString().Substring(2);
                code += _months[dateOfBirth.Month - 1];
                if (genre == 'M' || genre == 'm') code += (dateOfBirth.Day <= 9) ? "0" + dateOfBirth.Day.ToString() : dateOfBirth.Day.ToString();
                else if (genre == 'F' || genre == 'f') code += (dateOfBirth.Day + 40).ToString();
                else throw new NotSupportedException("ERROR: genere must be either 'M' or 'F'.");
                return code;
            }

            /// <summary>
            /// Calcola le 3 lettere del nome indicato, utilizzate per il calcolo del Codice Fiscale.
            /// </summary>
            /// <param name="firstName">Il nome della persona</param>
            /// <returns>Le 3 lettere che saranno utilizzate per il calcolo del Codice Fiscale</returns>
            private static string _calculateFromFirstName(string firstName)
            {
                firstName = _normalize(firstName, true);
                string code = string.Empty;
                string cons = string.Empty;
                int i = 0;
                while ((cons.Length < 4) && (i < firstName.Length))
                {
                    for (int j = 0; j < _consonants.Length; j++)
                        if (firstName[i] == _consonants[j]) cons = cons + firstName[i];
                    i++;
                }
                code = (cons.Length > 3)
                    // if we have 4 or more consonants we need to pick 1st, 3rd and 4th
                    ? cons[0].ToString() + cons[2].ToString() + cons[3].ToString()
                    // otherwise we pick them all
                    : code = cons;
                i = 0;
                // add Vocals (if needed)
                while ((code.Length < 3) && (i < firstName.Length))
                {
                    for (int j = 0; j < _vocals.Length; j++)
                        if (firstName[i] == _vocals[j]) code += firstName[i];
                    i++;
                }
                // add trailing X (if needed)
                return (code.Length < 3) ? code.PadRight(3, 'X') : code;
            }

            /// <summary>
            /// Calcola le 3 lettere del cognome indicato, utilizzate per il calcolo del Codice Fiscale.
            /// </summary>
            /// <param name="lastName">Il cognome della persona</param>
            /// <returns>Le 3 lettere che saranno utilizzate per il calcolo del Codice Fiscale</returns>
            private static string _calculateFromLastName(string lastName)
            {
                lastName = _normalize(lastName, true);
                string code = string.Empty;
                int i = 0;
                // pick Consonants
                while ((code.Length < 3) && (i < lastName.Length))
                {
                    for (int j = 0; j < _consonants.Length; j++)
                        if (lastName[i] == _consonants[j]) code += lastName[i];
                    i++;
                }
                i = 0;
                // pick Vocals (if needed)
                while (code.Length < 3 && i < lastName.Length)
                {
                    for (int j = 0; j < _vocals.Length; j++)
                        if (lastName[i] == _vocals[j]) code += lastName[i];
                    i++;
                }
                // add trailing X (if needed)
                return (code.Length < 3) ? code.PadRight(3, 'X') : code;
            }

            /// <summary>
            /// Costruisce un codice fiscale "formalmente corretto" sulla base dei parametri indicati.
            /// 
            /// - Il codice ISTAT, relativo al comune di nascita, può essere recuperato da questo elenco:
            ///   http://www.agenziaentrate.gov.it/wps/content/Nsilib/Nsi/Strumenti/Codici+attivita+e+tributo/Codici+territorio/Comuni+italia+esteri/
            ///   
            /// IMPORTANTE: Si ricorda che il Codice Fiscale generato potrebbe non corrispondere effettivamente a quello reale.
            /// </summary>
            /// <param name="firstName">Nome</param>
            /// <param name="lastName">Cognome</param>
            /// <param name="dateOfBirth">Data di nascita</param>
            /// <param name="genre">Genere ('M' o 'F')</param>
            /// <param name="istatCode">Codice ISTAT (1 lettera e 3 numeri. Es.: H501 per Roma)</param>
            /// <returns>Un Codice Fiscale "formalmente corretto", calcolato sulla base dei parametri indicati.</returns>
            public static string CalculateFiscalCode(string firstName, string lastName, DateTime dateOfBirth, char genre, string istatCode)
            {
                if (String.IsNullOrEmpty(firstName)) throw new NotSupportedException("ERRORE: Il parametro 'nome' è obbligatorio.");
                if (String.IsNullOrEmpty(lastName)) throw new NotSupportedException("ERRORE: Il parametro 'cognome' è obbligatorio.");
                if (genre != 'M' && genre != 'F') throw new NotSupportedException("ERRORE: Il parametro 'genere' deve essere 'M' oppure 'F'.");
                if (String.IsNullOrEmpty(istatCode)) throw new NotSupportedException("ERRORE: Il parametro 'codiceISTAT' è obbligatorio.");

                string cf = String.Format("{0}{1}{2}{3}",
                                             _calculateFromLastName(lastName),
                                             _calculateFromFirstName(firstName),
                                             _calculateDateOfBirthGenre(dateOfBirth, genre),
                                             istatCode
                                            );
                cf += _calculateCheckDigit(cf);
                return cf;
            }

            /// <summary>
            /// Effettua un "controllo formale" del Codice Fiscale indicato secondo i seguenti criteri:
            /// 
            /// - Controlla che non sia un valore nullo/vuoto.
            /// - Controlla che il codice sia coerente con le specifiche normative per i Codici Fiscali (inclusi possibili casi di omocodia).
            /// - Controlla che il carattere di controllo sia coerente rispetto al Codice Fiscale indicato.
            /// 
            /// IMPORTANTE: Si ricorda che, anche se il Codice Fiscale risulta "formalmente corretto", 
            /// non ci sono garanzie che si tratti di un Codice Fiscale relativo a una persona realmente esistente o esistita.
            /// </summary>
            /// <param name="fiscalCode">il codice fiscale da controllare</param>
            /// <returns>TRUE se il codice è formalmente corretto, FALSE in caso contrario</returns>
            public static bool CheckFiscalCode(string fiscalCode)
            {
                if (String.IsNullOrEmpty(fiscalCode) || fiscalCode.Length < 16) return false;
                fiscalCode = _normalize(fiscalCode, false);
                if (!_checkRegEx.Match(fiscalCode).Success)
                {
                    // Regex failed: it can be either an omocode or an invalid Fiscal Code
                    string cf_NoOmocodia = _omocodeReplace(fiscalCode);
                    if (!_checkRegEx.Match(cf_NoOmocodia).Success) return false; // invalid Fiscal Code
                }
                return fiscalCode[15] == _calculateCheckDigit(fiscalCode.Substring(0, 15));
            }

            /// <summary>
            /// Effettua un "controllo formale" del Codice Fiscale indicato secondo i seguenti criteri:
            /// 
            /// - Controlla che non sia un valore nullo/vuoto.
            /// - Controlla che il codice sia coerente con le specifiche normative per i Codici Fiscali (inclusi possibili casi di omocodia).
            /// - Controlla che il carattere di controllo sia coerente rispetto al Codice Fiscale indicato.
            /// - Controlla la corrispondenza tra il codice fiscale e i dati anagrafici indicati.
            /// 
            /// IMPORTANTE: Si ricorda che, anche se il Codice Fiscale risulta "formalmente corretto", 
            /// non ci sono garanzie che si tratti di un Codice Fiscale relativo a una persona realmente esistente o esistita.
            /// </summary>
            /// <param name="fiscalCode">il codice fiscale da controllare</param>
            /// <param name="firstName">Nome</param>
            /// <param name="lastName">Cognome</param>
            /// <param name="dateOfBirth">Data di nascita</param>
            /// <param name="genre">Genere ('M' o 'F')</param>
            /// <param name="istatCode">Codice ISTAT (1 lettera e 3 numeri. Es.: H501 per Roma)</param>
            /// <returns>TRUE se il codice è formalmente corretto, FALSE in caso contrario</returns>
            public static Boolean CheckFiscalCode(string fiscalCode, string firstName, string lastName, DateTime dateOfBirth, char genre, string istatCode)
            {
                if (String.IsNullOrEmpty(fiscalCode) || fiscalCode.Length < 16) return false;
                fiscalCode = _normalize(fiscalCode, false);
                string cf_NoOmocodia = string.Empty;
                if (!_checkRegEx.Match(fiscalCode).Success)
                {
                    // Regex failed: it can be either an omocode or an invalid Fiscal Code
                    cf_NoOmocodia = _omocodeReplace(fiscalCode);
                    if (!_checkRegEx.Match(cf_NoOmocodia).Success) return false; // invalid Fiscal Code
                }
                else cf_NoOmocodia = fiscalCode;

                // NOTE: 
                // - 'fc' è il codice fiscale inserito (potrebbe contenere lettere al posto di numeri per omocodia)
                // - 'cf_NoOmocodia' è il codice fiscale epurato di eventuali modifiche dovute a omocodia.

                if (String.IsNullOrEmpty(firstName) || cf_NoOmocodia.Substring(3, 3) != _calculateFromFirstName(firstName)) return false;
                if (String.IsNullOrEmpty(lastName) || cf_NoOmocodia.Substring(0, 3) != _calculateFromLastName(lastName)) return false;
                if (cf_NoOmocodia.Substring(6, 5) != _calculateDateOfBirthGenre(dateOfBirth, genre)) return false;
                if (String.IsNullOrEmpty(istatCode) || cf_NoOmocodia.Substring(11, 4) != _normalize(istatCode, false)) return false;

                // Il carattere di controllo, in caso di omocodia, è anch'esso calcolato sul codice fiscale modificato, quindi occorre utilizzare quest'ultimo.
                if (fiscalCode[15] != _calculateCheckDigit(fiscalCode.Substring(0, 15))) return false;

                return true;
            }
        }
    }
}