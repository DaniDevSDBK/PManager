using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PManager.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Clase que genera contraseñas aleatorias basadas en ciertas condiciones.
    /// </summary>
    public class PasswordGenerator
    {
        private string password = "";
        public string Password { get => password; set => password = value; }

        private List<string> _Capitals = new List<string>();
        private List<string> _LowerCase = new List<string>();
        private List<string> _Numbers = new List<string>();
        private List<string> _SpecialCharacters = new List<string>();
        private List<string> _FinalList = new List<string>();

        /// <summary>
        /// Constructor por defecto de PasswordGenerator.
        /// </summary>
        public PasswordGenerator()
        {

        }

        /// <summary>
        /// Constructor de PasswordGenerator que genera una contraseña basada en las condiciones proporcionadas.
        /// </summary>
        /// <param name="conditions">Lista de condiciones para generar la contraseña.</param>
        public PasswordGenerator(List<string> conditions)
        {
            Password = GenPassword(conditions);
        }

        private string GenPassword(List<string> conditions)
        {
            Random random = new Random();

            // Length
            StringBuilder pwd = new StringBuilder(int.Parse(conditions[0]));

            // Quantity Of Numbers
            if (int.Parse(conditions[1]) != 0)
            {
                FillList(1);
                FillFinalList(int.Parse(conditions[1]), _Numbers);
            }

            // Capitals Numbers
            if (int.Parse(conditions[2]) != 0)
            {
                FillList(2);
                FillFinalList(int.Parse(conditions[0]) - int.Parse(conditions[1]) - int.Parse(conditions[3]), _Capitals);
            }

            // Lower Case Numbers
            if (int.Parse(conditions[3]) != 0)
            {
                FillList(3);
                FillFinalList(int.Parse(conditions[0]) - int.Parse(conditions[1]) - int.Parse(conditions[2]), _LowerCase);
            }

            // Special Characters
            if (bool.Parse(conditions[4]))
            {
                FillList(0);
                FillFinalList(int.Parse(conditions[0]) - int.Parse(conditions[1]) - int.Parse(conditions[2]) - int.Parse(conditions[3]), _SpecialCharacters);
            }

            // Random OrderBy
            foreach (var item in _FinalList.OrderBy(_ => random.Next()).ToList())
            {
                pwd.Append(item);
            }

            return pwd.ToString();
        }

        private void FillList(int type)
        {
            switch (type)
            {
                case 0:
                    // Special Characters
                    for (int i = 32; i < 48; i++)
                    {
                        _SpecialCharacters.Add("" + (char)i);
                    }

                    for (int i = 58; i < 65; i++)
                    {
                        _SpecialCharacters.Add("" + (char)i);
                    }

                    for (int i = 91; i < 97; i++)
                    {
                        _SpecialCharacters.Add("" + (char)i);
                    }

                    for (int i = 123; i < 129; i++)
                    {
                        _SpecialCharacters.Add("" + (char)i);
                    }
                    break;

                case 1:
                    // Numbers
                    for (int i = 48; i < 58; i++)
                    {
                        _Numbers.Add("" + (char)i);
                    }
                    break;

                case 2:
                    // Capitals
                    for (int i = 65; i < 91; i++)
                    {
                        _Capitals.Add("" + (char)i);
                    }
                    break;

                case 3:
                    // Lower Case
                    for (int i = 97; i < 123; i++)
                    {
                        _LowerCase.Add("" + (char)i);
                    }
                    break;

                default:
                    break;
            }
        }

        private void FillFinalList(int n, List<string> list)
        {
            int count = 0;
            while (count < n)
            {
                _FinalList.Add(list[GenRandom(0, list.Count)]);
                count++;
            }
        }

        private int GenRandom(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
