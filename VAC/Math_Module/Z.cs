﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Module
{
    public class Z : Math_Field
    {
        #region Конструкторы

        public Z(List<string> s)
        {
            if (s[0].Contains("-") && s[0].Length != 1)
            {
                s[0] = s[0].Remove(0, 1);
                isN = false;
            }
            else if (s[0].Contains("-") && s[0].Length == 1)
            {
                s.RemoveAt(0);
                isN = false;
            }
            else
                isN = true;

            Abs = new N(s);
        }

        public Z(N value) // Александр Рассохин 9370 // Проверено Игорь
        {
            Abs = new N(value);
            isN = true;
        }

        #endregion

        #region Поля

        private N Abs;
        private bool isN;

        #endregion

        #region Свойства

        public override bool isDown // Евгений Куликов 9370//есть тесты
        {
            get
            {
                return isN;
            }
        }

        public N ABS_Z_N // Евгений Куликов 9370//есть тесты  
        {
            get
            {
                return new N(Abs);
            }
        }

        public byte POZ_Z_D // Евгений Куликов 9370//есть тесты
        {
            get
            {
                List<string> s = new List<string>();
                s.Add("0");
                if (N.COM_NN_D(Abs, new N(s)) == 0)
                {
                    return 0;
                }
                if (isN)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }

        #endregion

        #region Перегрузки

        public static Z operator -(Z value) // MUL_ZM_Z Евгений Куликов 9370//есть тесты
        {
            Z Clone = value.Clone();
            Clone.isN = !Clone.isN;
            return Clone;
        }

        public static Z operator +(Z first, Z second) // ADD_ZZ_Z Александр Баталин 9370//есть тесты
        {
            Z sum = first.Clone();
            if(first.isN != second.isN)
            {
                if (N.COM_NN_D(first.Abs, second.Abs) == 2)
                {
                    sum.Abs -= second.Abs;
                }
                else
                {
                    sum.Abs = second.Abs - first.Abs;
                    sum.isN = second.isN;
                }
            }
            else
                sum.Abs += second.Abs;
            return sum;
        }

        public static Z operator -(Z first, Z second) // SUB_ZZ_Z Александр Баталин 9370//(есть тесты не до конца)
        {
            return first + (-second);
        }

        public static Z operator *(Z first, Z second) // MUL_ZZ_Z//есть тесты
        {
            Z mult = first.Clone();
            mult.Abs *= second.Abs;
            if (first.isN != second.isN)
            {
                mult.isN = false;
            }
            else
            {
                mult.isN = true;
            } 
            return mult;
        } 

        public static Z operator /(Z first, Z second) // DIV_ZZ_Z //есть тесты
        {
             List<string> zero = new List<string>(); //создаем список с нулём
            zero.Add("0");
            Z div = new Z(zero); //зануляем перменную счётчик
            div.isN = true;
            if (N.COM_NN_D(first.Abs, second.Abs) != 1) //если второе небольше первого
            {
                if (second.POZ_Z_D != 0) //проверка на ноль
                {
                    if (N.COM_NN_D(first.Abs, second.Abs) == 2) //если первое больше второго
                    {
                        Z res = first.Clone(); //задём временную переменную
                        while (N.COM_NN_D(res.Abs, second.Abs) == 2) //пока первое больше второго
                        {
                            res.Abs -= second.Abs; //выполняем последовательное вычитание
                            div.Abs++; //наращиваем div
                        }
                        if (first.isN != second.isN)
                            div.isN = false; //присваимваем знак в случае их различия у делителя и делимого
                        return div; //возвращаем div
                    }
                    else //если первое равно второму
                    {
                        List<string> one = new List<string>(); //создаем новый список 
                        one.Add("1"); //добавляем туда единицу
                        Z result = new Z(one); //задаём значение result
                        if (first.isN != second.isN)
                            result.isN = false; //присваимваем знак в случае их различия у делителя и делимого
                        return result; //возвращаем result
                    }
                }
                else //если ноль возвращаем null
                    return null;
            }
            else //если второе больше первого
            {
                if (first.POZ_Z_D != 0) //проверка на ноль
                {
                    Z res = second.Clone(); //задём времнную переменную
                    while (N.COM_NN_D(res.Abs, first.Abs) == 2) //пока первое больше второго
                    {
                        res.Abs -= first.Abs; //выполняем последовательное вычитание
                        div.Abs++; //наращиваем div
                    }
                    if (first.isN != second.isN)
                        div.isN = false; //присваимваем знак в случае их различия у делителя и делимого
                    return div; //возвращаем div
                }
                else //если ноль возвращаем null
                    return null;
            }
        }

        public static Z operator %(Z first, Z second) // MOD_ZZ_Z //есть тесты
        {
            return null;
        }

        public static implicit operator List<string>(Z value)//есть тесты
        {
            List<string> S = value.Abs;
            
            if (value.isN == false)
            {
                if (S[0].Length < N.uint_size_div)
                {
                    System.Text.StringBuilder temp = new System.Text.StringBuilder();
                    temp.Append(Convert.ToString("-"));
                    temp.Append(Convert.ToString(S[0]));
                    S[0] = Convert.ToString(temp);
                }
                else if (S[0].Length == N.uint_size_div)
                    S.Insert(0, "-");
            }
            return S;
        }

        public static implicit operator Q(Z value) // Александр Рассохин
        {
            return new Q(value);
        }

        public static explicit operator N(Z value) // Александр Рассохин
        {
            if (!value.isDown)
                return null;
            else
                return new N(value.Abs);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == GetType() && this != null && obj != null)
            {
                Z sec = obj as Z;
                if (Abs.Equals(sec.Abs) && isN.Equals(sec.isN)) return true;
            }
            return false;
        }

        #endregion

        #region Методы

        public Z Clone() // Александр Баталин 9370//
        {
            Z clone = new Z(Abs.Clone());
            clone.isN = isN; 
            return clone;
        }


        #endregion
    }
}
